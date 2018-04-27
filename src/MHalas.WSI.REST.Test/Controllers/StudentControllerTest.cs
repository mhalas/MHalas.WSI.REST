using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MHalas.WSI.Lab1.Rest.Controllers;
using System.Linq;
using MHalas.WSI.Lab1.Models;

namespace MHalas.WSI.Lab1.Rest.Tests.Controllers
{
    [TestClass]
    public class StudentControllerTest
    {
        [TestMethod]
        public void Get_AllStudents_Success()
        {
            var controller = new StudentsController();
            controller.Items.Clear();
            controller.Items.Add(new Models.Student()
            {
                Identity = "138553",
                Name = "Maciej",
                Surname = "Halas",
                BirthDate = new DateTime(1992, 05, 19)
            });
            var result = controller.Get();

            Assert.AreEqual(result.Count(), 1);
        }

        [TestMethod]
        public void Get_StudentByIndex_Success()
        {
            var controller = new StudentsController();
            controller.Items.Add(new Models.Student()
            {
                Identity = "138553",
                Name = "Maciej",
                Surname = "Halas",
                BirthDate = new DateTime(1992, 05, 19)
            });

            var addedStudent = controller.Get("138553");

            Assert.IsNotNull(addedStudent);
        }

        [TestMethod]
        public void Post_AddStudent_Success()
        {
            var controller = new StudentsController();
            controller.Post(new Models.Student()
            {
                Identity = "138553",
                Name = "Maciej",
                Surname = "Halas",
                BirthDate = new DateTime(1992, 05, 19)
            });
            var result = controller.Items.Where(x => x.Identity == "138553").SingleOrDefault();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Put_EditStudent_Success()
        {
            var studentToEdit = new Models.Student()
            {
                Identity = "138553",
                Name = "Maciej",
                Surname = "Halas",
                BirthDate = new DateTime(1992, 05, 19)
            };

            var controller = new StudentsController();
            controller.Items.Clear();
            controller.Items.Add(studentToEdit);

            studentToEdit.BirthDate = new DateTime(2018, 01, 01);
            studentToEdit.Name = "Changed";
            studentToEdit.Surname = "Changed";

            var putResult = controller.Put(studentToEdit);
            Assert.IsNotNull(putResult);
            var getResult = controller.Get(studentToEdit.Identity);

            Assert.AreEqual("Changed", getResult.Name);
            Assert.AreEqual("Changed", getResult.Surname);
            Assert.AreEqual(new DateTime(2018,01,01), getResult.BirthDate);
        }

        [TestMethod]
        public void Put_EditNotExistingStudent_ReturnsNull()
        {
            var studentToEdit = new Models.Student()
            {
                Identity = "138553",
                Name = "Maciej",
                Surname = "Halas",
                BirthDate = new DateTime(1992, 05, 19)
            };
            var controller = new StudentsController();
            controller.Items.Clear();
            controller.Items.Add(studentToEdit);

            var putResult = controller.Put(new Student() { Identity = "x", Name="Changed", Surname="Changed", BirthDate = new DateTime(2000,02,02)});
            Assert.IsNull(putResult);
        }

        [TestMethod]
        public void Delete_DeleteStudent_Success()
        {
            var controller = new StudentsController();
            controller.Items.Clear();
            controller.Items.Add(new Models.Student()
            {
                Identity = "138553",
                Name = "Maciej",
                Surname = "Halas",
                BirthDate = new DateTime(1992, 05, 19)
            });
            Assert.AreEqual(1, controller.Items.Count());
            var result = controller.Delete("138553");
            Assert.AreEqual(0, controller.Items.Count());
            Assert.IsNull(result);
        }
    }
}
