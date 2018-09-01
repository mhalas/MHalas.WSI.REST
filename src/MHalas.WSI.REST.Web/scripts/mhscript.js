"use strict";
var protocol = "http://";
var serverAddress = "localhost";
var port = ":59688";
var apiPath = "/api/";

var studentsList;
var studentCoursesList;
var studentCourseGradesList;
var coursesList;

var studentEditor;
var studentDetails;
var studentCourseEditor;
var studentCourseGradeEditor;
var gradeEditor;
var courseEditor;
var courseDetails;

var StudentViewModel = function() {
  var self = this;

  this.Id = ko.observable();
  this.Index = ko.observable();
  this.Grades = ko.observable();
  this.BirthDate = ko.observable();
  this.FirstName = ko.observable();
  this.LastName = ko.observable();

  this.isEdit = ko.computed(function() {
    if(this.Id())
      return true;

    return false
  }, this);
};
var CourseViewModel = function() {
  var self = this;

  this.Id = ko.observable();
  this.ECTS = ko.observable();
  this.LeadTeacher = ko.observable();
  this.Name = ko.observable();

  this.CourseId = ko.observable();
};
var GradeViewModel = function() {
  var self = this;

  this.CourseId = ko.observable();
  this.StudentId = ko.observable();
  this.Value = ko.observable();
};
var StudentCourseViewModel = function() {
  var self = this;

  this.StudentIndex = ko.observable();
  this.Course = new CourseViewModel();

  this.CourseList = new ListViewModel();
};
var StudentCourseGradeViewModel = function() {
  var self = this;

  this.StudentIndex = ko.observable();
  this.CourseID = ko.observable();
  this.GradeValue = ko.observable();
}
var ListViewModel = function() {
  var self = this;
  this.list = ko.observableArray();
  this.parentId = ko.observable();

  this.IndexFilter = ko.observable();
  this.FirstnameFilter = ko.observable();
  this.LastnameFilter = ko.observable();
  this.BirthDateFromFilter = ko.observable();
  this.BirthDateToFilter = ko.observable();

  this.NameFilter = ko.observable();
  this.LeadTeacherFilter = ko.observable();
  this.ECTSFilter = ko.observable();
}
var ErrorViewModel = function() {
  this.Message = ko.observable();
  this.ExceptionMessage = ko.observable();
}

$(document).ready(function() {
  coursesList = new ListViewModel();
  studentsList = new ListViewModel();
  studentCoursesList = new ListViewModel();
  studentCourseGradesList = new ListViewModel();

  studentEditor = new StudentViewModel();
  studentDetails = new StudentViewModel();
  studentCourseEditor = new StudentCourseViewModel();
  studentCourseGradeEditor = new StudentCourseGradeViewModel();
  gradeEditor = new GradeViewModel();
  courseEditor = new CourseViewModel();
  courseDetails = new CourseViewModel();

  ko.applyBindings(coursesList, $("#courses")[0]);
  ko.applyBindings(studentsList, $("#students")[0]);
  ko.applyBindings(studentCoursesList, $("#student-courses")[0]);
  ko.applyBindings(studentCourseGradesList, $("#student-course-grades")[0]);
  
  ko.applyBindings(studentEditor, $("#student-editor")[0]);
  ko.applyBindings(studentCourseEditor, $("#student-course-editor")[0]);
  ko.applyBindings(studentCourseGradeEditor, $("#student-course-grade-editor")[0]);
  ko.applyBindings(studentDetails, $("#student-details")[0]);

  ko.applyBindings(courseDetails, $("#course-details")[0]);
  ko.applyBindings(courseEditor, $("#course-editor")[0]);

  $('.tableHeaderFilter').keypress(function(e){
    if(e.which == 13){
        $(this).blur();
    }
  });

  $(".studentsFilter").focusout(function(){
    GetStudents();
  });
  $(".coursesFilter").focusout(function(){
    GetCourses();
  });
  $(".studentCoursesFilter").focusout(function(){
    GetStudentCourses(studentCoursesList.parentId());
  });

  $('#student-form').submit(function(e) {
    GetStudents();
    window.location.href = '#students';
  });

  $('#course-form').submit(function(e) {
    GetCourses();
    window.location.href = '#courses';
  });
  $('#student-course-form').submit(function(e) {
    GetStudentCourses(studentCoursesList.parentId());
    window.location.href = '#student-courses';
  });
  $('#student-course-grade-form').submit(function(e) {
    GetCourseGrades(studentCourseGradesList.parentId());
    window.location.href = '#student-course-grades';
  });
});

function GetDataFromAPI(controllerName, method, vm) {
  if(!method) {
    method = "";
  }

  var parameters = "";
  parameters = AddParameter(parameters, "Firstname", vm.FirstnameFilter());
  parameters = AddParameter(parameters, "Lastname", vm.LastnameFilter());
  parameters = AddParameter(parameters, "Index", vm.IndexFilter());
  parameters = AddParameter(parameters, "BirthdateFrom", vm.BirthDateFromFilter());
  parameters = AddParameter(parameters, "BirthdateTo", vm.BirthDateToFilter());

  parameters = AddParameter(parameters, "Name", vm.NameFilter());
  parameters = AddParameter(parameters, "LeadTeacher", vm.LeadTeacherFilter());
  parameters = AddParameter(parameters, "ECTS", vm.ECTSFilter());

  $.ajax({
    url: protocol + serverAddress + port + apiPath + controllerName + method + parameters,
    method: "GET",
    async: false,
    "accept": "application/json",
    success: function (data) {
      ko.mapping.fromJS(data, {}, vm.list);
    },
    error: function (error) {
      var errorVM = new ErrorViewModel();
      var obj = JSON.parse(error.responseText);
      ko.mapping.fromJS(obj, {}, errorVM);
      alert(errorVM.Message() + "\n" + errorVM.ExceptionMessage());
    }
  });
}
function AddParameter(parameterString, propertyName, value) {
  var result = "";

  if(!value) {
    return parameterString;
  }

  if(!parameterString) {
    result = "?";
  }
  else {
    result = parameterString;
  }

  if(result == "?") {
    result = result + propertyName + "=" + value;
  }
  else {
    result = result + "&" + propertyName + "=" + value;
  }

  return result;
}

function Create(controllerName, method, vm) {
  if(!method) {
    method = "";
  }

  var obj = ko.mapping.toJS(vm);
  var json = JSON.stringify(obj);

  $.ajax({
    url: protocol + serverAddress + port + apiPath + controllerName + method,
    method: "POST",
    async: false,
    data: json,
    contentType: "application/json",
    success: function (data) {
      alert("Create success!");
    },
    error: function (error) {
      var errorVM = new ErrorViewModel();
      var obj = JSON.parse(error.responseText);
      ko.mapping.fromJS(obj, {}, errorVM);
      alert(errorVM.Message() + "\n" + errorVM.ExceptionMessage());
    }
  });
}
function Update(controllerName, method, vm, id) {
  if(!method) {
    method = "";
  }

  var obj = ko.mapping.toJS(vm);
  var json = JSON.stringify(obj);

  $.ajax({
    url: protocol + serverAddress + port + apiPath + controllerName + method + "/" + id,
    method: "PUT",
    async: false,
    data: json,
    contentType: "application/json",
    success: function (data) {
      alert("Update success!");
    },
    error: function (error) {
      var errorVM = new ErrorViewModel();
      var obj = JSON.parse(error.responseText);
      ko.mapping.fromJS(obj, {}, errorVM);
      alert(errorVM.Message() + "\n" + errorVM.ExceptionMessage());
    }
  });
}
function DeleteObject(path) {
  $.ajax({
    url: protocol + serverAddress + port + apiPath + path,
    method: "DELETE",
    async: false,
    "contentType": "application/json",
    success: function (data) {
      alert("Delete success.");
    },
    error: function (error) {
      var errorVM = new ErrorViewModel();
      var obj = JSON.parse(error.responseText);
      ko.mapping.fromJS(obj, {}, errorVM);
      alert(errorVM.Message() + "\n" + errorVM.ExceptionMessage());
    }
  });
}
function ClearFilter(vm) {
  vm.IndexFilter("");
  vm.FirstnameFilter("");
  vm.LastnameFilter("");

  vm.NameFilter("");
  vm.LeadTeacherFilter("");
  vm.ECTSFilter("");
}

function GetStudents() {
  GetDataFromAPI('students', null, studentsList);
}
function CreateUpdateStudent() {
  if(studentEditor.Id()) {
    Update("students", null, studentEditor, studentEditor.Index());
  }
  else {
    Create("students", null, studentEditor);
  }
}
function GetStudentDetails(index) {
  var vm = studentsList.list()[index];
  MapStudentVM(studentDetails, vm);
}
function GetStudentEditor(index) {
  var vm = studentsList.list()[index];
  MapStudentVM(studentEditor, vm);
}
function ClearStudentEditor() {
  MapStudentVM(studentEditor, new StudentViewModel());
}
function DeleteStudent(index) {
  var respond = confirm("Do you want to delete this object?");
  if(respond) {
    DeleteObject("students/"+index);
    GetStudents();
  }
}
function MapStudentVM(vm1, vm2) {
  vm1.Id(vm2.Id());
  vm1.Index(vm2.Index());
  vm1.Grades(vm2.Grades());
  vm1.BirthDate(vm2.BirthDate());
  vm1.FirstName(vm2.FirstName());
  vm1.LastName(vm2.LastName());
}

function GetStudentCourses(index) {
  GetDataFromAPI('students', 
  '/'+index+'/courses', 
  studentCoursesList);
  studentCoursesList.parentId(index);
}
function CreateStudentCourse() {
  var studentId = studentCoursesList.parentId();
  var vm = studentCourseEditor.Course;

  Create("students", "/" + studentId + "/courses", vm);
}
function GetStudentCourseEditor() {
  studentCourseEditor.StudentIndex(studentCoursesList.parentId());
  GetCourses();
  studentCourseEditor.CourseList.list(coursesList.list());
}
function DeleteStudentCourse(id) {
  var respond = confirm("Do you want to delete this object?");
  if(respond) {
    var studentIndex = studentCoursesList.parentId();
    DeleteObject("students/" + studentIndex + "/courses/" + id);
    GetStudentCourses(studentIndex);
  }
}

function GetStudentCourseGradeEditor() {
  studentCourseGradeEditor.StudentIndex(studentCoursesList.parentId());
  studentCourseGradeEditor.CourseID(studentCourseGradesList.parentId());
}
function CreateStudentCourseGrade() {
  var studentId = studentCoursesList.parentId();
  var courseId = studentCourseGradesList.parentId();
  Create("students", "/" + studentId + "/courses/" + courseId + "/grades/", studentCourseGradeEditor);
  GetCourseGrades(courseId);
}
function GetCourseGrades(id) {
  var studentCourseId = studentCoursesList.parentId();

  GetDataFromAPI('students', 
  '/'+studentCourseId+'/courses/'+id+'/grades', 
  studentCourseGradesList);
  studentCourseGradesList.parentId(id);
}
function DeleteStudentCourseGrade(id) {
  var respond = confirm("Do you want to delete this object?");
  if(respond) {
    var studentIndex = studentCoursesList.parentId();
    var courseId = studentCourseGradesList.parentId();
    DeleteObject("students/" + studentIndex + "/courses/"+ courseId + "/grades/" + id);
    GetCourseGrades(courseId);
  }
}

function GetCourses() {
  GetDataFromAPI('courses', null, coursesList);
}
function CreateUpdateCourse() {
  if(courseEditor.Id()) {
    Update("courses", null, courseEditor, courseEditor.Id());
  }
  else {
    Create("courses", null, courseEditor);
  }
}
function GetCourseDetails(index) {
  var vm = coursesList.list()[index];
  MapCourseVM(courseDetails, vm);
}
function GetCourseEditor(index) {
  var vm = coursesList.list()[index];
  MapCourseVM(courseEditor, vm);
}
function DeleteCourse(index) {
  var respond = confirm("Do you want to delete this course?");
  if(respond) {
    DeleteObject("courses/"+index);
    GetCourses();
  }
}
function ClearCourseEditor() {
  MapCourseVM(courseEditor, new CourseViewModel());
}
function MapCourseVM(vm1, vm2) {
  vm1.Id(vm2.Id());
  vm1.ECTS(vm2.ECTS());
  vm1.LeadTeacher(vm2.LeadTeacher());
  vm1.Name(vm2.Name());
}
