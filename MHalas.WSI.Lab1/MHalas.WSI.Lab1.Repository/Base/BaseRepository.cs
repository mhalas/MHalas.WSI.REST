using MHalas.WSI.Lab1.Configuration;
using MHalas.WSI.Lab1.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MHalas.WSI.Lab1.Repository.Base
{
    public class BaseRepository<TModel>
        where TModel : IId<ObjectId>
    {
        public string _collectionName;
        public string _databaseName;
        public string _databaseConnectionString;
        private MongoClient _client;

        public BaseRepository(string collectionName)
        {
            _databaseName = DBConfig.DatabaseName;
            _databaseConnectionString = DBConfig.ConnectionString;

            _collectionName = collectionName;
            _client = new MongoClient(_databaseConnectionString);
        }

        public bool StartSession()
        {
            if (_client.Cluster.Description.State == ClusterState.Connected)
                return true;
            else
                _client.StartSession();

            return _client.Cluster.Description.State == ClusterState.Connected;
        }


        private IMongoCollection<TModel> GetCollection()
        {
            StartSession();
            return _client.GetDatabase(_databaseName).GetCollection<TModel>(_collectionName);
        }
        public virtual void Create(TModel newObject)
        {
            StartSession();

            try
            {
                GetCollection().InsertOne(newObject);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public virtual void Create(List<TModel> list)
        {
            StartSession();

            try
            {
                GetCollection().InsertMany(list);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public virtual IEnumerable<TModel> Retrieve(List<MongoDBRef> dbRefs)
        {
            StartSession();
            var ids = dbRefs.Select(x => x.Id);
            return GetCollection().Find(x => ids.Any(z => z == x.Id)).ToList();
        }
        public virtual IEnumerable<TModel> Retrieve(Expression<Func<TModel, bool>> filter = null)
        {
            StartSession();
            if(filter == null)
                return GetCollection().Find(FilterDefinition<TModel>.Empty).ToList();
            else
                return GetCollection().Find(filter).ToList();
        }
        public virtual IEnumerable<TModel> Retrieve(FilterDefinition<TModel> filter)
        {
            StartSession();
            return GetCollection().Find(filter).ToList();
        }
        public virtual ReplaceOneResult Update(Expression<Func<TModel, bool>> filter, TModel updateDefinition)
        {
            StartSession();
            var objectToUpdate = Retrieve(filter).SingleOrDefault();
            if (objectToUpdate == null)
                throw new Exception($"Cant find object where '{filter.Body}'.");

            updateDefinition.Id = objectToUpdate.Id;

            return GetCollection().ReplaceOne(filter, updateDefinition);
        }
        public virtual DeleteResult Delete(Expression<Func<TModel, bool>> filter)
        {
            StartSession();
            return GetCollection().DeleteOne(filter);
        }

        public virtual UpdateResult FindOneAndUpdate(
            Expression<Func<TModel, IEnumerable<MongoDBRef>>> dbRefExpression,
            ObjectId parentObjectId,
            MongoDBRef dbRef)
        {
            return FindOneAndUpdate<MongoDBRef>(dbRefExpression, parentObjectId, dbRef);
        }
        public virtual UpdateResult FindOneAndUpdate<ParentObjectListItem>(
            Expression<Func<TModel, IEnumerable<ParentObjectListItem>>> parentArrayExpression, 
            ObjectId parentObjectId, 
            ParentObjectListItem item)
        {
            var filter = Builders<TModel>.Filter.Eq(x => x.Id, parentObjectId);
            var updateBuilder = Builders<TModel>.Update.Push(parentArrayExpression, item);
            return GetCollection().UpdateOne(filter, updateBuilder);
        }
    }
}
