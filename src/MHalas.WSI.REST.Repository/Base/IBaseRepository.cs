﻿using MHalas.WSI.REST.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MHalas.WSI.REST.Repository.Base
{
    public interface IBaseRepository<TModel> where TModel : IId<ObjectId>
    {
        void Create(List<TModel> list);
        void Create(TModel newObject);
        DeleteResult Delete(Expression<Func<TModel, bool>> filter);
        UpdateResult FindOneAndUpdate(Expression<Func<TModel, IEnumerable<MongoDBRef>>> dbRefExpression, ObjectId parentObjectId, MongoDBRef dbRef);
        UpdateResult FindOneAndUpdate<ParentObjectListItem>(Expression<Func<TModel, IEnumerable<ParentObjectListItem>>> parentArrayExpression, ObjectId parentObjectId, ParentObjectListItem item);
        IEnumerable<TModel> Retrieve(Expression<Func<TModel, bool>> filter = null);
        IEnumerable<TModel> Retrieve(FilterDefinition<TModel> filter);
        IEnumerable<TModel> Retrieve(List<MongoDBRef> dbRefs);
        bool StartSession();
        UpdateResult Update(FilterDefinition<TModel> filter, UpdateDefinition<TModel> updateDefinition);
    }
}