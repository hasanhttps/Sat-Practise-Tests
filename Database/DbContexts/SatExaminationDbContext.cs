using System;
using System.Linq;
using Database.Configurations;
using Database.Entities.Concretes;
using Microsoft.EntityFrameworkCore;

namespace Database.DbContexts;

public class SatExaminationDbContext : DbContext {
 
    // Configurations

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        // Data Source=192.168.1.77,1433;Initial Catalog=SatExam;User ID=sadmin;Password=admin;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False
        optionsBuilder.UseLazyLoadingProxies(true).UseSqlServer("Data Source=192.168.1.77,1433;Initial Catalog=SatExam;User ID=sadmin;Password=admin;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder){

        modelBuilder.ApplyConfiguration(new ExamConfiguration());
        modelBuilder.ApplyConfiguration(new ModuleConfiguration());
        modelBuilder.ApplyConfiguration(new AnswerConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionConfiguration());
        modelBuilder.ApplyConfiguration(new SatStudentConfiguration());
        modelBuilder.ApplyConfiguration(new ExamResultConfiguration());
        modelBuilder.ApplyConfiguration(new StudentsAnswersConfiguration());
        modelBuilder.ApplyConfiguration(new StudentsExamResultsConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    // Tables

    public virtual DbSet<Exam> Exams { get; set; }
    public virtual DbSet<Module> Modules { get; set; }
    public virtual DbSet<Answer> Answers { get; set; }
    public virtual DbSet<Question> Questions { get; set; }
    public virtual DbSet<ExamResult> ExamResults { get; set; }
    public virtual DbSet<SatStudent> SatStudents { get; set; }
    public virtual DbSet<StudentsAnswers> StudentsAnswers { get; set; }
    public virtual DbSet<StudentsExamResults> StudentsExamResults { get; set; }
}