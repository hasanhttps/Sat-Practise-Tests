using System;
using System.Linq;
using Database.DbContexts;
using System.Collections.Generic;
using Database.Entities.Concretes;

namespace Application.Models.DatabaseNamespace;

public static class Database {

    // Properties

    public static SatStudent CurrentStudent { get; set; }
    public static List<SatStudent> SatStudents { get; set; }
    public static SatExaminationDbContext DbContext { get; set; }

    // Constructor

    static Database() {

    }

    // Functions

    public static bool CheckStudentExist(int schoolnumber) {

        foreach (var student in SatStudents) {
            if (student.SchoolNumber == schoolnumber) {
                CurrentStudent = student;
                return true;
            }
        }
        return false;
    }
}