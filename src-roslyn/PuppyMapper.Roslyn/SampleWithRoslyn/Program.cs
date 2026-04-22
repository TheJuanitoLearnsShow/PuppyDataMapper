using SampleWithRoslyn;

var mapper = new StudentMapper(new Student(), new Score());
mapper.DoMapping();