using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Data;
using BusinessLogic.Services;

namespace BusinessLogic.Seeders
{
    public class DataSeeder
    {
        private readonly SchoolDbContext _context;

        public DataSeeder(SchoolDbContext context)
        {
            _context = context;
        }

        public void Seed(IPasswordHashService passwordHashService)
        {
            if (!_context.Database.CanConnect()) return;

            if (!_context.Users.Any())
            {
                SeedUsers(passwordHashService);
            }

            if (!_context.ClassStudents.Any())
            {
                SeedClasses();
            }

            if (!_context.WeeklySchedules.Any())
            {
                SeedWeeklySchedule();
            }

            if (!_context.Lessons.Any())
            {
                SeedOperationalData();
            }

            if (!_context.Announcements.Any())
            {
                SeedAnnouncements();
            }
        }

        // dane nauczycieli
        private List<(int SubjectId, string FirstName, string LastName, string EmailPrefix)> GetTeachersData()
        {
            return new List<(int, string, string, string)>
            {
                (1, "Anna", "Kowalska", "akowalska"),
                (2, "Jan", "Nowak", "jnowak"),
                (3, "Ewa", "Wiśniewska", "ewisniewska"),
                (4, "Piotr", "Kamiński", "pkaminski"),
                (5, "Marek", "Lewandowski", "mlewandowski"),
                (6, "Katarzyna", "Zielińska", "kzielinska"),
                (7, "Michał", "Szymański", "mszymanski"),
                (8, "Agnieszka", "Woźniak", "awozniak"),
                (9, "Tomasz", "Dąbrowski", "tdabrowski"),
                (10, "Paweł", "Kozłowski", "pkozlowski"),
                (11, "Małgorzata", "Jankowska", "mjankowska"),
                (12, "Joanna", "Mazur", "jmazur"),
                (13, "Grzegorz", "Wojciechowski", "gwojciechowski"),
                (14, "Barbara", "Kwiatkowska", "bkwiatkowska"),
                (15, "Łukasz", "Krawczyk", "lkrawczyk"),
                (16, "Dorota", "Piotrowska", "dpiotrowska"),
                (17, "Marcin", "Grabowski", "mgrabowski"),
                (18, "Elżbieta", "Pawłowska", "epawlowska"),
                (19, "Rafał", "Michalski", "rmichalski"),
                (20, "Karolina", "Król", "kkrol"),
                (21, "Krzysztof", "Wieczorek", "kwieczorek")
            };
        }

        // dane klasy 1
        private List<(string StudentFirstName, string LastName, string StudentPrefix, string ParentFirstName, string ParentPrefix)> GetClass1StudentsData()
        {
            return new List<(string, string, string, string, string)>
            {
                ("Leon", "Urbaniak", "lurbaniak", "Marek", "murbaniak"),
                ("Pola", "Sikora", "psikora", "Ewa", "esikora"),
                ("Oliwier", "Baran", "obaran", "Adam", "abaran"),
                ("Laura", "Krajewska", "lkrajewska", "Monika", "mkrajewska"),
                ("Nikodem", "Mróz", "nmroz", "Piotr", "pmroz"),
                ("Iga", "Wróblewska", "iwroblewska", "Anna", "awroblewska"),
                ("Tymon", "Głowacki", "tglowacki", "Krzysztof", "kglowacki"),
                ("Marcelina", "Zakrzewska", "mzakrzewska", "Maria", "mazakrzewska"),
                ("Ignacy", "Laskowski", "ilaskowski", "Paweł", "plaskowski"),
                ("Klara", "Makowska", "kmakowska", "Zofia", "zmakowska")
            };
        }

        // dane klasy 2
        private List<(string StudentFirstName, string LastName, string StudentPrefix, string ParentFirstName, string ParentPrefix)> GetClass2StudentsData()
        {
            return new List<(string, string, string, string, string)>
            {
                ("Kacper", "Dudek", "kdudek", "Tomasz", "tdudek"),
                ("Natalia", "Adamczyk", "nadamczyk", "Magdalena", "madamczyk"),
                ("Mateusz", "Wieczorek", "mwieczorek", "Andrzej", "awieczorek"),
                ("Karolina", "Stępień", "kstepien", "Joanna", "jstepien"),
                ("Bartosz", "Pawlak", "bpawlak", "Grzegorz", "gpawlak"),
                ("Weronika", "Walczak", "wwalczak", "Barbara", "bwalczak"),
                ("Dawid", "Sikorski", "dsikorski", "Robert", "rsikorski"),
                ("Martyna", "Sobczak", "msobczak", "Agnieszka", "asobczak"),
                ("Kamil", "Drzewiecki", "kdrzewiecki", "Dariusz", "ddrzewiecki"),
                ("Patrycja", "Malinowska", "pmalinowska", "Katarzyna", "kmalinowska")
            };
        }

        private void SeedUsers(IPasswordHashService passwordHashService)
        {
            var users = new List<User>();
            var password = "Test123!";

            var admin = new User
            {
                FirstName = "Krzysztof",
                LastName = "Jarzyna",
                Email = "admin@szkola.edu.pl",
                Password = passwordHashService.HashPassword(password),
                Phone = "111111111",
                Street = "Szkolna 1",
                City = "Warszawa",
                PostalCode = "00-001",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            users.Add(admin);

            var teachersData = GetTeachersData();
            foreach (var t in teachersData)
            {
                users.Add(new User
                {
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    Email = $"{t.EmailPrefix}@szkola.edu.pl",
                    Password = passwordHashService.HashPassword(password),
                    Phone = $"600100{t.SubjectId:00}",
                    Street = $"Nauczycielska {t.SubjectId}",
                    City = "Warszawa",
                    PostalCode = "00-002",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            _context.Users.AddRange(users);
            _context.SaveChanges();

            var staffRoles = new List<UserRole>();
            var teacherEmails = teachersData.Select(t => $"{t.EmailPrefix}@szkola.edu.pl").ToHashSet();

            foreach (var user in users)
            {
                if (user.Email == "admin@szkola.edu.pl")
                    staffRoles.Add(new UserRole
                    {
                        UserId = user.Id,
                        RoleId = 1,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                else if (teacherEmails.Contains(user.Email))
                    staffRoles.Add(new UserRole
                    {
                        UserId = user.Id,
                        RoleId = 2,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
            }
            _context.UserRoles.AddRange(staffRoles);
            _context.SaveChanges();

            var allStudentsData = GetClass1StudentsData().Concat(GetClass2StudentsData()).ToList();
            int phoneCounter = 0;
            int addressCounter = 1;

            foreach (var s in allStudentsData)
            {
                var street = $"Kwiatowa {addressCounter}";
                var city = "Warszawa";
                var postalCode = $"00-{100 + addressCounter}";

                var student = new User
                {
                    FirstName = s.StudentFirstName,
                    LastName = s.LastName,
                    Email = $"{s.StudentPrefix}@szkola.edu.pl",
                    Password = passwordHashService.HashPassword(password),
                    Phone = $"700{phoneCounter:0000}",
                    Street = street,
                    City = city,
                    PostalCode = postalCode,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var parent = new User
                {
                    FirstName = s.ParentFirstName,
                    LastName = s.LastName,
                    Email = $"{s.ParentPrefix}@szkola.edu.pl",
                    Password = passwordHashService.HashPassword(password),
                    Phone = $"800{phoneCounter:0000}",
                    Street = street,
                    City = city,
                    PostalCode = postalCode,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(student);
                _context.Users.Add(parent);
                _context.SaveChanges();

                _context.ParentStudents.Add(new ParentStudent
                {
                    StudentId = student.Id,
                    ParentId = parent.Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

                _context.UserRoles.Add(new UserRole
                {
                    UserId = student.Id,
                    RoleId = 4,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
                _context.UserRoles.Add(new UserRole
                {
                    UserId = parent.Id,
                    RoleId = 3,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

                phoneCounter++;
                addressCounter++;
            }

            _context.SaveChanges();
        }

        private void SeedClasses()
        {
            // klasa 1
            var class1Data = GetClass1StudentsData();
            var class1Emails = class1Data.Select(s => $"{s.StudentPrefix}@szkola.edu.pl").ToList();
            var students1 = _context.Users.Where(u => class1Emails.Contains(u.Email)).ToList();

            foreach (var s in students1)
                _context.ClassStudents.Add(new ClassStudent
                {
                    ClassId = 1,
                    StudentId = s.Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

            // klasa 2
            var class2Data = GetClass2StudentsData();
            var class2Emails = class2Data.Select(s => $"{s.StudentPrefix}@szkola.edu.pl").ToList();
            var students2 = _context.Users.Where(u => class2Emails.Contains(u.Email)).ToList();

            foreach (var s in students2)
                _context.ClassStudents.Add(new ClassStudent
                {
                    ClassId = 2,
                    StudentId = s.Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

            // nauczyciele i przedmioty
            var teachersData = GetTeachersData();
            var teachersMap = new Dictionary<int, int>();

            foreach (var t in teachersData)
            {
                var email = $"{t.EmailPrefix}@szkola.edu.pl";
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    teachersMap[t.SubjectId] = user.Id;
                }
            }

            // przedmioty 1
            var subjectsClass1 = new[] { 1, 2, 3, 4, 5, 13, 14, 16 };
            foreach (var subjId in subjectsClass1)
            {
                if (teachersMap.ContainsKey(subjId))
                {
                    _context.ClassSubjects.Add(new ClassSubject
                    {
                        ClassId = 1,
                        SubjectId = subjId,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                    _context.TeacherClassSubjects.Add(new TeacherClassSubject
                    {
                        ClassId = 1,
                        SubjectId = subjId,
                        TeacherId = teachersMap[subjId],
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

            // przedmioty 2
            var subjectsClass2 = new[] { 1, 2, 3, 5, 6, 8, 9, 10, 11, 20 };
            foreach (var subjId in subjectsClass2)
            {
                if (teachersMap.ContainsKey(subjId))
                {
                    _context.ClassSubjects.Add(new ClassSubject
                    {
                        ClassId = 2,
                        SubjectId = subjId,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                    _context.TeacherClassSubjects.Add(new TeacherClassSubject
                    {
                        ClassId = 2,
                        SubjectId = subjId,
                        TeacherId = teachersMap[subjId],
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

            _context.SaveChanges();
        }

        private void SeedWeeklySchedule()
        {
            var schedules = new List<WeeklySchedule>();
            var teachersData = GetTeachersData();
            var subjectToEmail = teachersData.ToDictionary(t => t.SubjectId, t => $"{t.EmailPrefix}@szkola.edu.pl");

            int GetTeacherId(int subjId)
            {
                if (!subjectToEmail.ContainsKey(subjId))
                    throw new Exception($"Brak nauczyciela dla przedmiotu: {subjId}");

                var email = subjectToEmail[subjId];
                return _context.Users.First(u => u.Email == email).Id;
            }

            // klasa 1a
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 1, LessonHourId = 1, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 1, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 1, LessonHourId = 2, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 2, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 1, LessonHourId = 3, SubjectId = 5, TeacherId = GetTeacherId(5), ClassroomId = 16, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 1, LessonHourId = 4, SubjectId = 3, TeacherId = GetTeacherId(3), ClassroomId = 3, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 2, LessonHourId = 1, SubjectId = 3, TeacherId = GetTeacherId(3), ClassroomId = 3, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 2, LessonHourId = 2, SubjectId = 16, TeacherId = GetTeacherId(16), ClassroomId = 4, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 2, LessonHourId = 3, SubjectId = 14, TeacherId = GetTeacherId(14), ClassroomId = 5, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 2, LessonHourId = 4, SubjectId = 13, TeacherId = GetTeacherId(13), ClassroomId = 5, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 3, LessonHourId = 1, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 2, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 3, LessonHourId = 2, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 1, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 3, LessonHourId = 3, SubjectId = 4, TeacherId = GetTeacherId(4), ClassroomId = 15, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 3, LessonHourId = 4, SubjectId = 5, TeacherId = GetTeacherId(5), ClassroomId = 16, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 4, LessonHourId = 1, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 2, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 4, LessonHourId = 2, SubjectId = 3, TeacherId = GetTeacherId(3), ClassroomId = 3, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 4, LessonHourId = 3, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 1, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 4, LessonHourId = 4, SubjectId = 16, TeacherId = GetTeacherId(16), ClassroomId = 4, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 5, LessonHourId = 1, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 1, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 5, LessonHourId = 2, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 2, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 5, LessonHourId = 3, SubjectId = 15, TeacherId = GetTeacherId(15), ClassroomId = 5, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 5, LessonHourId = 4, SubjectId = 5, TeacherId = GetTeacherId(5), ClassroomId = 16, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

            // klasa 8c
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 1, LessonHourId = 1, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 6, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 1, LessonHourId = 2, SubjectId = 10, TeacherId = GetTeacherId(10), ClassroomId = 7, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 1, LessonHourId = 3, SubjectId = 9, TeacherId = GetTeacherId(9), ClassroomId = 8, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 1, LessonHourId = 4, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 9, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 1, LessonHourId = 5, SubjectId = 3, TeacherId = GetTeacherId(3), ClassroomId = 10, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 1, LessonHourId = 6, SubjectId = 5, TeacherId = GetTeacherId(5), ClassroomId = 17, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 1, LessonHourId = 7, SubjectId = 6, TeacherId = GetTeacherId(6), ClassroomId = 11, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 2, LessonHourId = 1, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 9, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 2, LessonHourId = 2, SubjectId = 6, TeacherId = GetTeacherId(6), ClassroomId = 11, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 2, LessonHourId = 3, SubjectId = 8, TeacherId = GetTeacherId(8), ClassroomId = 8, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 2, LessonHourId = 4, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 6, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 2, LessonHourId = 5, SubjectId = 11, TeacherId = GetTeacherId(11), ClassroomId = 12, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 2, LessonHourId = 6, SubjectId = 4, TeacherId = GetTeacherId(4), ClassroomId = 15, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 2, LessonHourId = 7, SubjectId = 7, TeacherId = GetTeacherId(7), ClassroomId = 13, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 3, LessonHourId = 2, SubjectId = 3, TeacherId = GetTeacherId(3), ClassroomId = 10, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 3, LessonHourId = 3, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 6, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 3, LessonHourId = 4, SubjectId = 10, TeacherId = GetTeacherId(10), ClassroomId = 7, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 3, LessonHourId = 5, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 9, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 3, LessonHourId = 6, SubjectId = 5, TeacherId = GetTeacherId(5), ClassroomId = 17, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 3, LessonHourId = 7, SubjectId = 20, TeacherId = GetTeacherId(20), ClassroomId = 14, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 4, LessonHourId = 1, SubjectId = 9, TeacherId = GetTeacherId(9), ClassroomId = 8, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 4, LessonHourId = 2, SubjectId = 8, TeacherId = GetTeacherId(8), ClassroomId = 8, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 4, LessonHourId = 3, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 6, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 4, LessonHourId = 4, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 9, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 4, LessonHourId = 5, SubjectId = 3, TeacherId = GetTeacherId(3), ClassroomId = 10, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 4, LessonHourId = 6, SubjectId = 11, TeacherId = GetTeacherId(11), ClassroomId = 12, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 4, LessonHourId = 7, SubjectId = 5, TeacherId = GetTeacherId(5), ClassroomId = 17, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 5, LessonHourId = 1, SubjectId = 7, TeacherId = GetTeacherId(7), ClassroomId = 13, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 5, LessonHourId = 2, SubjectId = 6, TeacherId = GetTeacherId(6), ClassroomId = 11, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 5, LessonHourId = 3, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 9, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 5, LessonHourId = 4, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 6, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 5, LessonHourId = 5, SubjectId = 10, TeacherId = GetTeacherId(10), ClassroomId = 7, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 5, LessonHourId = 6, SubjectId = 3, TeacherId = GetTeacherId(3), ClassroomId = 10, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

            _context.WeeklySchedules.AddRange(schedules);
            _context.SaveChanges();
        }

        private void SeedOperationalData()
        {
            var teachersData = GetTeachersData();
            var subjectToEmail = teachersData.ToDictionary(t => t.SubjectId, t => $"{t.EmailPrefix}@szkola.edu.pl");

            int GetTeacherId(int subjId)
            {
                if (!subjectToEmail.ContainsKey(subjId))
                    throw new Exception($"Nie znaleziono nauczyciela dla przedmiotu ID: {subjId}");

                var email = subjectToEmail[subjId];
                return _context.Users.First(u => u.Email == email).Id;
            }

            // lekcje klasy 1
            var lessons1A = new List<Lesson>
            {
                new Lesson { ClassId = 1, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 1, LessonHourId = 1, Topic = "Liczby", StatusId = 2, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Date = DateTime.UtcNow },
                new Lesson { ClassId = 1, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 2, LessonHourId = 2, Topic = "Alfabet", StatusId = 2, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Date = DateTime.UtcNow },
                new Lesson { ClassId = 1, SubjectId = 5, TeacherId = GetTeacherId(5), ClassroomId = 16, LessonHourId = 3, Topic = "Gimnastyka", StatusId = 2, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Date = DateTime.UtcNow },
                new Lesson { ClassId = 1, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 1, LessonHourId = 1, Topic = "Dodawanie i odejmowanie", StatusId = 2, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Date = DateTime.UtcNow },
                new Lesson { ClassId = 1, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 2, LessonHourId = 2, Topic = "Czytanie", StatusId = 2, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Date = DateTime.UtcNow }
            };
            _context.Lessons.AddRange(lessons1A);

            // lekcje klasy 2
            var lessons8C = new List<Lesson>
            {
                new Lesson { ClassId = 2, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 6, LessonHourId = 1, Topic = "Funkcja liniowa", StatusId = 2, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Date = DateTime.UtcNow },
                new Lesson { ClassId = 2, SubjectId = 10, TeacherId = GetTeacherId(10), ClassroomId = 7, LessonHourId = 2, Topic = "Ruch jednostajny", StatusId = 2, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Date = DateTime.UtcNow },
                new Lesson { ClassId = 2, SubjectId = 9, TeacherId = GetTeacherId(9), ClassroomId = 8, LessonHourId = 3, Topic = "Kwasy i zasady", StatusId = 2, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Date = DateTime.UtcNow },
                new Lesson { ClassId = 2, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 6, LessonHourId = 1, Topic = "Układy równań", StatusId = 2, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Date = DateTime.UtcNow },
                new Lesson { ClassId = 2, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 9, LessonHourId = 2, Topic = "Omówienie lektury Pan Tadeusz", StatusId = 2, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Date = DateTime.UtcNow }
            };
            _context.Lessons.AddRange(lessons8C);
            _context.SaveChanges();

            var students1A = _context.ClassStudents.Where(cs => cs.ClassId == 1).Select(cs => cs.StudentId).ToList();
            var students8C = _context.ClassStudents.Where(cs => cs.ClassId == 2).Select(cs => cs.StudentId).ToList();

            void FillStudentData(List<int> studentIds, List<Lesson> lessons)
            {
                foreach (var studId in studentIds)
                {
                    // frekwencja
                    _context.Attendances.Add(new Attendance { StudentId = studId, LessonId = lessons[0].Id, AttendanceTypeId = 2, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                    _context.Attendances.Add(new Attendance { StudentId = studId, LessonId = lessons[1].Id, AttendanceTypeId = 2, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                    _context.Attendances.Add(new Attendance { StudentId = studId, LessonId = lessons[2].Id, AttendanceTypeId = 3, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                    _context.Attendances.Add(new Attendance { StudentId = studId, LessonId = lessons[3].Id, AttendanceTypeId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                    _context.Attendances.Add(new Attendance { StudentId = studId, LessonId = lessons[4].Id, AttendanceTypeId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

                    // oceny
                    _context.Grades.Add(new Grade { StudentId = studId, TeacherId = lessons[0].TeacherId, SubjectId = lessons[0].SubjectId, GradeTypeId = 3, GradeCategoryId = 1, Comment = "", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, DateTime = DateTime.UtcNow });
                    _context.Grades.Add(new Grade { StudentId = studId, TeacherId = lessons[1].TeacherId, SubjectId = lessons[1].SubjectId, GradeTypeId = 4, GradeCategoryId = 2, Comment = "OK", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, DateTime = DateTime.UtcNow });
                    _context.Grades.Add(new Grade { StudentId = studId, TeacherId = lessons[2].TeacherId, SubjectId = lessons[2].SubjectId, GradeTypeId = 5, GradeCategoryId = 4, Comment = "Gratulacje", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, DateTime = DateTime.UtcNow });
                    _context.Grades.Add(new Grade { StudentId = studId, TeacherId = lessons[3].TeacherId, SubjectId = lessons[3].SubjectId, GradeTypeId = 2, GradeCategoryId = 5, Comment = "", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, DateTime = DateTime.UtcNow });
                }
            }

            FillStudentData(students1A, lessons1A);
            FillStudentData(students8C, lessons8C);

            _context.SaveChanges();
        }

        private void SeedAnnouncements()
        {
            var adminId = _context.UserRoles.First(ur => ur.RoleId == 1).UserId;

            var teacherIds = _context.UserRoles
                .Where(ur => ur.RoleId == 2)
                .Select(ur => ur.UserId)
                .Take(3)
                .ToList();

            var announcements = new List<Announcement>
            {
                new Announcement
                {
                    Title = "Wpłaty na radę rodziców",
                    Description = "Szanowni Państwo, przypominamy o konieczności uiszczenia opłaty na Radę Rodziców do 18 października. Wpłaty można dokonywać na konto bankowe szkoły lub w sekretariacie",
                    CreatedAt = new DateTime(2025, 10, 12),
                    UpdatedAt = new DateTime(2025, 10, 12),
                    AuthorId = adminId,
                    IsActive = true
                },
                new Announcement
                {
                    Title = "Dzień Nauczyciela",
                    Description = "W związku z obchodami Dnia Edukacji Narodowej, w dniu 14 października lekcje zostają skrócone. Świetlica pracuje bez zmian",
                    CreatedAt = new DateTime(2025, 10, 7),
                    UpdatedAt = new DateTime(2025, 10, 7),
                    AuthorId = teacherIds[0],
                    IsActive = true
                },
                new Announcement
                {
                    Title = "Konkurs matematyczny 'Kangur'",
                    Description = "Zapraszamy wszystkich chętnych uczniów klas 4-8 do udziału w międzynarodowym konkursie matematycznym Kangur. Zapisy u nauczycieli matematyki do końca tygodnia.",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2),
                    AuthorId = teacherIds[1],
                    IsActive = true
                },
                new Announcement
                {
                    Title = "Zebranie rodziców klas 1-3",
                    Description = "Zapraszamy na zebranie rodziców klas 1-3, odbędzie się w najbliższy wtorek o godzinie 17:00 w auli.",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1),
                    AuthorId = teacherIds[2],
                    IsActive = true
                }
            };

            _context.Announcements.AddRange(announcements);
            _context.SaveChanges();
        }
    }
}