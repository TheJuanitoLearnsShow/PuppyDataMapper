using SampleWithRoslyn;

var student = new Student();
var score = new Score()
{
    TestScore = 90,
    IsStudent = true,
    Weight = 0.5m,
    Test1 = new TestData() { Name = "Test1"},
    Test2 = new TestData() { Name = "Test2"},
};
var mapper = new StudentMapper(student, score);
mapper.DoMapping();
Console.WriteLine(student.StudentName);
Console.WriteLine(student.GPA);