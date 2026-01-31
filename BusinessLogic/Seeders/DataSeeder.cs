namespace BusinessLogic.Seeders
{
    using BusinessLogic.Services;
    using Data.Data;
    using Data.Data.Entities;
    using Microsoft.EntityFrameworkCore;

    public class DataSeeder
    {
        private readonly EduPlusDbContext _context;
        private readonly Random _random = new Random();

        private const string adminMail = "eduplus.test.1@gmail.com";
        private const string teacherMail = "eduplus.test.2@gmail.com";
        private const string password = "Test123!";

        public DataSeeder(EduPlusDbContext context)
        {
            _context = context;
        }

        private string GeneratePhoneNumber()
        {
            return _random.Next(500000000, 899999999).ToString();
        }

        public void Seed(IPasswordHashService passwordHashService)
        {
            if (!_context.Database.CanConnect()) return;

            if (!_context.Classrooms.Any())
            {
                SeedClassrooms();
            }

            if (!_context.Subjects.Any())
            {
                SeedSubjects();
            }

            if (!_context.Classes.Any())
            {
                SeedBaseClasses();
            }

            if (!_context.Users.Any())
            {
                SeedUsers(passwordHashService);
            }

            if (!_context.SubjectTeachers.Any())
            {
                SeedSubjectTeachers();
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

            if (!_context.Tickets.Any())
            {
                SeedTickets();
            }
        }

        private List<(int SubjectId, string FirstName, string LastName, string Email, string Street, string City, string PostalCode)> GetTeachersData()
        {
            return new List<(int, string, string, string, string, string, string)>
            {
                (1, "Anna", "Kowalska", teacherMail, "Złota 44/12", "Warszawa", "00-120"),
                (2, "Jan", "Nowak", "jnowak@szkola.edu.pl", "Marszałkowska 85/3", "Warszawa", "00-683"),
                (3, "Ewa", "Wiśniewska", "ewisniewska@szkola.edu.pl", "Aleje Jerozolimskie 100", "Warszawa", "00-807"),
                (4, "Piotr", "Kamiński", "pkaminski@szkola.edu.pl", "Chmielna 5", "Warszawa", "00-021"),
                (5, "Marek", "Lewandowski", "mlewandowski@szkola.edu.pl", "Nowy świat 22", "Warszawa", "00-373"),
                (6, "Katarzyna", "Zielińska", "kzielinska@szkola.edu.pl", "Puławska 15", "Warszawa", "02-515"),
                (7, "Michał", "Szymański", "mszymanski@szkola.edu.pl", "Wilanowska 200", "Warszawa", "02-765"),
                (8, "Agnieszka", "Woźniak", "awozniak@szkola.edu.pl", "Francuska 12", "Warszawa", "03-906"),
                (9, "Tomasz", "Dąbrowski", "tdabrowski@szkola.edu.pl", "Targowa 67", "Warszawa", "03-729"),
                (10, "Paweł", "Kozłowski", "pkozlowski@szkola.edu.pl", "Grójecka 45", "Warszawa", "02-031"),
                (11, "Małgorzata", "Jankowska", "mjankowska@szkola.edu.pl", "Wojska Polskiego 10", "Pruszków", "05-800"),
                (12, "Joanna", "Mazur", "jmazur@szkola.edu.pl", "Kościuszki 5", "Piaseczno", "05-500"),
                (13, "Grzegorz", "Wojciechowski", "gwojciechowski@szkola.edu.pl", "Piłsudskiego 99", "Marki", "05-270"),
                (14, "Barbara", "Kwiatkowska", "bkwiatkowska@szkola.edu.pl", "3 Maja 14", "Legionowo", "05-120"),
                (15, "Łukasz", "Krawczyk", "lkrawczyk@szkola.edu.pl", "Sienkiewicza 7", "Otwock", "05-400"),
                (16, "Dorota", "Piotrowska", "dpiotrowska@szkola.edu.pl", "Mickiewicza 2", "Ząbki", "05-091"),
                (17, "Marcin", "Grabowski", "mgrabowski@szkola.edu.pl", "Leśna 18", "Oleśnica", "05-092"),
                (18, "Elżbieta", "Pawłowska", "epawlowska@szkola.edu.pl", "Polna 33", "Wołomin", "05-200"),
                (19, "Rafał", "Michalski", "rmichalski@szkola.edu.pl", "Ogrodowa 11", "Sulejówek", "05-070"),
                (20, "Karolina", "Król", "kkrol@szkola.edu.pl", "Słoneczna 4", "Konstancin-Jeziorna", "05-520"),
                (21, "Krzysztof", "Wieczorek", "kwieczorek@szkola.edu.pl", "Kwiatowa 8", "Józefów", "05-420")
            };
        }

        private List<(string S_Name, string S_Last, string S_Email, string P_Name, string P_Email, string Street, string City, string PostalCode)> GetClass1StudentsData()
        {
            return new List<(string, string, string, string, string, string, string, string)>
            {
                ("Leon", "Urbaniak", "lurbaniak@szkola.edu.pl", "Marek", "murbaniak@szkola.edu.pl", "Długa 55", "Grodzisk Mazowiecki", "05-825"),
                ("Pola", "Sikora", "psikora@szkola.edu.pl", "Ewa", "esikora@szkola.edu.pl", "Krótka 1", "Mińsk Mazowiecki", "05-300"),
                ("Oliwier", "Baran", "obaran@szkola.edu.pl", "Adam", "abaran@szkola.edu.pl", "Spacerowa 9", "Milanów", "05-822"),
                ("Laura", "Krajewska", "lkrajewska@szkola.edu.pl", "Monika", "mkrajewska@szkola.edu.pl", "Wspólna 12", "Brwinów", "05-840"),
                ("Nikodem", "Mróz", "nmroz@szkola.edu.pl", "Piotr", "pmroz@szkola.edu.pl", "Lipowa 6", "Bonie", "05-870"),
                ("Iga", "Wróblewska", "iwroblewska@szkola.edu.pl", "Anna", "awroblewska@szkola.edu.pl", "Akacjowa 3", "Nadarzyn", "05-830"),
                ("Tymon", "Głowacki", "tglowacki@szkola.edu.pl", "Krzysztof", "kglowacki@szkola.edu.pl", "Brzozowa 21", "Raszyn", "05-090"),
                ("Marcelina", "Zakrzewska", "mzakrzewska@szkola.edu.pl", "Maria", "mazakrzewska@szkola.edu.pl", "Topolowa 15", "Zielonka", "05-220"),
                ("Ignacy", "Laskowski", "ilaskowski@szkola.edu.pl", "Paweł", "plaskowski@szkola.edu.pl", "Klonowa 7", "Kobyłka", "05-230"),
                ("Klara", "Makowska", "kmakowska@szkola.edu.pl", "Zofia", "zmakowska@szkola.edu.pl", "Dębowa 2", "Ożarów Mazowiecki", "05-850"),
                ("Antoni", "Czerwiński", "aczerwinski@szkola.edu.pl", "Robert", "rczerwinski@szkola.edu.pl", "Polna 5", "Grodzisk Mazowiecki", "05-825"),
                ("Maja", "Szymczak", "mszymczak@szkola.edu.pl", "Agnieszka", "aszymczak@szkola.edu.pl", "Szkolna 12", "Milanówek", "05-822"),
                ("Filip", "Bąk", "fbak@szkola.edu.pl", "Tomasz", "tbak@szkola.edu.pl", "Leśna 3", "Podkowa Leśna", "05-807"),
                ("Julia", "Włodarczyk", "jwlodarczyk@szkola.edu.pl", "Katarzyna", "kwlodarczyk@szkola.edu.pl", "Kwiatowa 8", "Brwinów", "05-840"),
                ("Szymon", "Dudziński", "sdudzinski@szkola.edu.pl", "Marcin", "mdudzinski@szkola.edu.pl", "Ogrodowa 20", "Błonie", "05-870"),
                ("Zuzanna", "Kaczmarek", "zkaczmarek@szkola.edu.pl", "Michał", "mikaczmarek@szkola.edu.pl", "Słoneczna 15", "Raszyn", "05-090"),
                ("Franciszek", "Kołodziej", "fkolodziej@szkola.edu.pl", "Wojciech", "wkolodziej@szkola.edu.pl", "Lipowa 2", "Nadarzyn", "05-830"),
                ("Lena", "Sobolewska", "lsobolewska@szkola.edu.pl", "Marta", "msobolewska@szkola.edu.pl", "Wiśniowa 9", "Ożarów Mazowiecki", "05-850"),
                ("Mikołaj", "Rogowski", "mrogowski@szkola.edu.pl", "Kamil", "krogowski@szkola.edu.pl", "Brzozowa 11", "Stare Babice", "05-082"),
                ("Alicja", "Witkowska", "awitkowska@szkola.edu.pl", "Piotr", "pwitkowski@szkola.edu.pl", "Zielona 4", "Leszno", "05-084"),
                ("Stanisław", "Lis", "slis@szkola.edu.pl", "Jacek", "jlis@szkola.edu.pl", "Miodowa 7", "Izabelin", "05-080"),
                ("Amelia", "Nawrocka", "anawrocka@szkola.edu.pl", "Ewelina", "enawrocka@szkola.edu.pl", "Parkowa 10", "Łomianki", "05-092"),
                ("Jakub", "Bednarek", "jbednarek@szkola.edu.pl", "Grzegorz", "gbednarek@szkola.edu.pl", "Wrzosowa 5", "Czosnów", "05-152"),
                ("Hanna", "Olejniczak", "holejniczak@szkola.edu.pl", "Dominika", "dolejniczak@szkola.edu.pl", "Sosnowa 14", "Leoncin", "05-155"),
                ("Wojciech", "Pietrzak", "wpietrzak@szkola.edu.pl", "Sławomir", "spietrzak@szkola.edu.pl", "Klonowa 33", "Nowy Dwór Mazowiecki", "05-100")
            };
        }

        private List<(string S_Name, string S_Last, string S_Email, string P_Name, string P_Email, string Street, string City, string PostalCode)> GetClass2StudentsData()
        {
            return new List<(string, string, string, string, string, string, string, string)>
            {
                ("Kacper", "Dudek", "kdudek@szkola.edu.pl", "Tomasz", "tdudek@szkola.edu.pl", "Sosnowa 19", "Karczew", "05-480"),
                ("Natalia", "Adamczyk", "nadamczyk@szkola.edu.pl", "Magdalena", "madamczyk@szkola.edu.pl", "Świerkowa 14", "Radzymin", "05-250"),
                ("Mateusz", "Wieczorek", "mwieczorek@szkola.edu.pl", "Andrzej", "awieczorek@szkola.edu.pl", "Jarzębinowa 5", "Tuszcz", "05-240"),
                ("Karolina", "Stępień", "kstepien@szkola.edu.pl", "Joanna", "jstepien@szkola.edu.pl", "Wrzosowa 8", "Góra Kalwaria", "05-530"),
                ("Bartosz", "Pawlak", "bpawlak@szkola.edu.pl", "Grzegorz", "gpawlak@szkola.edu.pl", "Różana 10", "Wesoła", "05-077"),
                ("Weronika", "Walczak", "wwalczak@szkola.edu.pl", "Barbara", "bwalczak@szkola.edu.pl", "Błękitna 3", "Wawer", "04-645"),
                ("Dawid", "Sikorski", "dsikorski@szkola.edu.pl", "Robert", "rsikorski@szkola.edu.pl", "Cicha 6", "Rembertów", "04-406"),
                ("Martyna", "Sobczak", "msobczak@szkola.edu.pl", "Agnieszka", "asobczak@szkola.edu.pl", "Spokojna 11", "Ursus", "02-495"),
                ("Kamil", "Drzewiecki", "kdrzewiecki@szkola.edu.pl", "Dariusz", "ddrzewiecki@szkola.edu.pl", "Wesoła 22", "Warszawa", "02-400"),
                ("Patrycja", "Malinowska", "pmalinowska@szkola.edu.pl", "Katarzyna", "kmalinowska@szkola.edu.pl", "Prosta 40", "Bemowo", "01-310"),
                ("Oskar", "Jasiński", "ojasinski@szkola.edu.pl", "Mariusz", "mjasinski@szkola.edu.pl", "Dębowa 7", "Sulejówek", "05-070"),
                ("Wiktoria", "Górska", "wgorska@szkola.edu.pl", "Iwona", "igorska@szkola.edu.pl", "Kasztanowa 12", "Halinów", "05-074"),
                ("Miłosz", "Sawicki", "msawicki@szkola.edu.pl", "Rafał", "rsawicki@szkola.edu.pl", "Jodłowa 3", "Józefów", "05-420"),
                ("Gabriela", "Kruk", "gkruk@szkola.edu.pl", "Dorota", "dkruk@szkola.edu.pl", "Topolowa 9", "Otwock", "05-400"),
                ("Hubert", "Szczepański", "hszczepanski@szkola.edu.pl", "Marek", "mszczepanski@szkola.edu.pl", "Wspólna 21", "Karczew", "05-480"),
                ("Zofia", "Kaźmierczak", "zkazmierczak@szkola.edu.pl", "Aneta", "akazmierczak@szkola.edu.pl", "Leśna 55", "Wiązowna", "05-462"),
                ("Maciej", "Skowroński", "mskowronski@szkola.edu.pl", "Krzysztof", "kskowronski@szkola.edu.pl", "Polna 8", "Kołbiel", "05-340"),
                ("Aleksandra", "Konieczna", "akonieczna@szkola.edu.pl", "Małgorzata", "makonieczna@szkola.edu.pl", "Warszawska 100", "Mińsk Mazowiecki", "05-300"),
                ("Adam", "Domagała", "adomagala@szkola.edu.pl", "Jan", "jdomagala@szkola.edu.pl", "Siedlecka 12", "Dąb Wielkie", "05-311"),
                ("Magdalena", "Wróbel", "mwrobel@szkola.edu.pl", "Bożena", "bwrobel@szkola.edu.pl", "Miła 2", "Stanisławów", "05-304"),
                ("Dominik", "Mazurek", "dmazurek@szkola.edu.pl", "Adam", "admazurek@szkola.edu.pl", "Cicha 15", "Wołomin", "05-200"),
                ("Emilia", "Zaręba", "ezareba@szkola.edu.pl", "Monika", "mzareba@szkola.edu.pl", "Piaskowa 7", "Kobyłka", "05-230"),
                ("Borys", "Piątek", "bpiatek@szkola.edu.pl", "Artur", "apiatek@szkola.edu.pl", "Słoneczna 19", "Marki", "05-270"),
                ("Blanka", "Grzelak", "bgrzelak@szkola.edu.pl", "Sylwia", "sgrzelak@szkola.edu.pl", "Graniczna 4", "Ząbki", "05-091"),
                ("Ksawery", "Łuczak", "kluczak@szkola.edu.pl", "Damian", "dluczak@szkola.edu.pl", "Mickiewicza 11", "Zielonka", "05-220")
            };
        }

        private void SeedUsers(IPasswordHashService passwordHashService)
        {
            var users = new List<User>();

            var admin = new User
            {
                FirstName = "Krzysztof",
                LastName = "Jarzyna",
                Email = adminMail,
                Password = passwordHashService.HashPassword(password),
                Phone = GeneratePhoneNumber(),
                Street = "Szkolna 1",
                City = "Warszawa",
                PostalCode = "00-001",
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ModifiedByUserId = null
            };
            users.Add(admin);

            var teachersData = GetTeachersData();
            foreach (var t in teachersData)
            {
                users.Add(new User
                {
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    Email = t.Email,
                    Password = passwordHashService.HashPassword(password),
                    Phone = GeneratePhoneNumber(),
                    Street = t.Street,
                    City = t.City,
                    PostalCode = t.PostalCode,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ModifiedByUserId = null
                });
            }

            _context.Users.AddRange(users);
            _context.SaveChanges();

            var staffRoles = new List<UserRole>();
            var teacherEmails = teachersData.Select(t => t.Email).ToHashSet();

            foreach (var user in users)
            {
                if (user.Email == adminMail)
                    staffRoles.Add(new UserRole
                    {
                        UserId = user.Id,
                        RoleId = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        ModifiedByUserId = null
                    });
                else if (teacherEmails.Contains(user.Email))
                    staffRoles.Add(new UserRole
                    {
                        UserId = user.Id,
                        RoleId = 2,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        ModifiedByUserId = null
                    });
            }
            _context.UserRoles.AddRange(staffRoles);
            _context.SaveChanges();

            var allStudentsData = GetClass1StudentsData().Concat(GetClass2StudentsData()).ToList();

            foreach (var s in allStudentsData)
            {
                var student = new User
                {
                    FirstName = s.S_Name,
                    LastName = s.S_Last,
                    Email = s.S_Email,
                    Password = passwordHashService.HashPassword(password),
                    Phone = GeneratePhoneNumber(),
                    Street = s.Street,
                    City = s.City,
                    PostalCode = s.PostalCode,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ModifiedByUserId = null
                };

                var parent = new User
                {
                    FirstName = s.P_Name,
                    LastName = s.S_Last,
                    Email = s.P_Email,
                    Password = passwordHashService.HashPassword(password),
                    Phone = GeneratePhoneNumber(),
                    Street = s.Street,
                    City = s.City,
                    PostalCode = s.PostalCode,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ModifiedByUserId = null
                };

                _context.Users.Add(student);
                _context.Users.Add(parent);
                _context.SaveChanges();

                _context.ParentStudents.Add(new ParentStudent
                {
                    StudentId = student.Id,
                    ParentId = parent.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ModifiedByUserId = null
                });

                _context.UserRoles.Add(new UserRole
                {
                    UserId = student.Id,
                    RoleId = 4,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ModifiedByUserId = null
                });
                _context.UserRoles.Add(new UserRole
                {
                    UserId = parent.Id,
                    RoleId = 3,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ModifiedByUserId = null
                });
            }

            _context.SaveChanges();
        }

        private void SeedSubjectTeachers()
        {
            var teachersData = GetTeachersData();
            var teacherEmails = teachersData.Select(t => t.Email).ToList();
            var dbTeachers = _context.Users.Where(u => teacherEmails.Contains(u.Email)).ToList();

            var subjectTeachers = new List<SubjectTeacher>();

            foreach (var t in teachersData)
            {
                var teacher = dbTeachers.FirstOrDefault(u => u.Email == t.Email);
                if (teacher != null)
                {
                    subjectTeachers.Add(new SubjectTeacher
                    {
                        TeacherId = teacher.Id,
                        SubjectId = t.SubjectId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true,
                        ModifiedByUserId = null
                    });
                }
            }

            _context.SubjectTeachers.AddRange(subjectTeachers);
            _context.SaveChanges();
        }

        private void SeedClasses()
        {
            var class1Data = GetClass1StudentsData();
            var class1Emails = class1Data.Select(s => s.S_Email).ToList();

            var students1 = _context.Users
                .Where(u => class1Emails.Contains(u.Email))
                .OrderBy(u => u.LastName)
                .ToList();

            int order1 = 1;

            foreach (var s in students1)
            {
                _context.ClassStudents.Add(new ClassStudent
                {
                    ClassId = 1,
                    StudentId = s.Id,
                    OrderNumber = order1++,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ModifiedByUserId = null
                });
            }

            var class2Data = GetClass2StudentsData();
            var class2Emails = class2Data.Select(s => s.S_Email).ToList();

            var students2 = _context.Users
                .Where(u => class2Emails.Contains(u.Email))
                .OrderBy(u => u.LastName)
                .ToList();

            int order2 = 1;

            foreach (var s in students2)
            {
                _context.ClassStudents.Add(new ClassStudent
                {
                    ClassId = 2,
                    StudentId = s.Id,
                    OrderNumber = order2++,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ModifiedByUserId = null
                });
            }

            var teachersData = GetTeachersData();
            var teachersMap = new Dictionary<int, int>();

            foreach (var t in teachersData)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == t.Email);
                if (user != null)
                {
                    teachersMap[t.SubjectId] = user.Id;
                }
            }

            var subjectsClass1 = new[] { 1, 2, 3, 4, 5, 13, 14, 16 };
            foreach (var subjId in subjectsClass1)
            {
                if (teachersMap.ContainsKey(subjId))
                {
                    _context.ClassSubjects.Add(new ClassSubject
                    {
                        ClassId = 1,
                        SubjectId = subjId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true,
                        ModifiedByUserId = null
                    });
                    _context.TeacherClassSubjects.Add(new TeacherClassSubject
                    {
                        ClassId = 1,
                        SubjectId = subjId,
                        TeacherId = teachersMap[subjId],
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true,
                        ModifiedByUserId = null
                    });
                }
            }

            var subjectsClass2 = new[] { 1, 2, 3, 5, 6, 8, 9, 10, 11, 20 };
            foreach (var subjId in subjectsClass2)
            {
                if (teachersMap.ContainsKey(subjId))
                {
                    _context.ClassSubjects.Add(new ClassSubject
                    {
                        ClassId = 2,
                        SubjectId = subjId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true,
                        ModifiedByUserId = null
                    });
                    _context.TeacherClassSubjects.Add(new TeacherClassSubject
                    {
                        ClassId = 2,
                        SubjectId = subjId,
                        TeacherId = teachersMap[subjId],
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true,
                        ModifiedByUserId = null
                    });
                }
            }

            _context.SaveChanges();
        }

        private void SeedWeeklySchedule()
        {
            var schedules = new List<WeeklySchedule>();
            var teachersData = GetTeachersData();

            var teacherEmails = teachersData.Select(t => t.Email).Distinct().ToList();

            var teachersMap = _context.Users
                .Where(u => teacherEmails.Contains(u.Email))
                .ToDictionary(u => u.Email, u => u.Id);

            var subjectToTeacherId = teachersData.ToDictionary(
                t => t.SubjectId,
                t => teachersMap[t.Email]
            );

            int GetTeacherId(int subjId)
            {
                return subjectToTeacherId[subjId];
            }

            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 1, LessonHourId = 1, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 1, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 1, LessonHourId = 2, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 2, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 1, LessonHourId = 3, SubjectId = 5, TeacherId = GetTeacherId(5), ClassroomId = 16, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 1, LessonHourId = 4, SubjectId = 3, TeacherId = GetTeacherId(3), ClassroomId = 3, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });

            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 2, LessonHourId = 1, SubjectId = 3, TeacherId = GetTeacherId(3), ClassroomId = 3, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 2, LessonHourId = 2, SubjectId = 16, TeacherId = GetTeacherId(16), ClassroomId = 4, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 2, LessonHourId = 3, SubjectId = 14, TeacherId = GetTeacherId(14), ClassroomId = 5, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 2, LessonHourId = 4, SubjectId = 13, TeacherId = GetTeacherId(13), ClassroomId = 5, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });

            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 3, LessonHourId = 1, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 2, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 3, LessonHourId = 2, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 1, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 3, LessonHourId = 3, SubjectId = 4, TeacherId = GetTeacherId(4), ClassroomId = 15, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 3, LessonHourId = 4, SubjectId = 5, TeacherId = GetTeacherId(5), ClassroomId = 16, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });

            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 4, LessonHourId = 1, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 2, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 4, LessonHourId = 2, SubjectId = 3, TeacherId = GetTeacherId(3), ClassroomId = 3, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 4, LessonHourId = 3, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 1, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 4, LessonHourId = 4, SubjectId = 16, TeacherId = GetTeacherId(16), ClassroomId = 4, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });

            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 5, LessonHourId = 1, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 1, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 5, LessonHourId = 2, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 2, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 5, LessonHourId = 3, SubjectId = 15, TeacherId = GetTeacherId(15), ClassroomId = 5, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 1, DayOfWeek = 5, LessonHourId = 4, SubjectId = 5, TeacherId = GetTeacherId(5), ClassroomId = 16, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });

            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 1, LessonHourId = 1, SubjectId = 10, TeacherId = GetTeacherId(10), ClassroomId = 7, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 1, LessonHourId = 2, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 6, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 1, LessonHourId = 3, SubjectId = 9, TeacherId = GetTeacherId(9), ClassroomId = 8, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 1, LessonHourId = 4, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 9, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 1, LessonHourId = 5, SubjectId = 3, TeacherId = GetTeacherId(3), ClassroomId = 10, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 1, LessonHourId = 6, SubjectId = 5, TeacherId = GetTeacherId(5), ClassroomId = 17, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 1, LessonHourId = 7, SubjectId = 6, TeacherId = GetTeacherId(6), ClassroomId = 11, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });

            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 2, LessonHourId = 1, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 9, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 2, LessonHourId = 2, SubjectId = 6, TeacherId = GetTeacherId(6), ClassroomId = 11, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 2, LessonHourId = 3, SubjectId = 8, TeacherId = GetTeacherId(8), ClassroomId = 8, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 2, LessonHourId = 4, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 6, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 2, LessonHourId = 5, SubjectId = 11, TeacherId = GetTeacherId(11), ClassroomId = 12, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 2, LessonHourId = 6, SubjectId = 4, TeacherId = GetTeacherId(4), ClassroomId = 15, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 2, LessonHourId = 7, SubjectId = 7, TeacherId = GetTeacherId(7), ClassroomId = 13, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });

            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 3, LessonHourId = 2, SubjectId = 3, TeacherId = GetTeacherId(3), ClassroomId = 10, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 3, LessonHourId = 3, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 6, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 3, LessonHourId = 4, SubjectId = 10, TeacherId = GetTeacherId(10), ClassroomId = 7, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 3, LessonHourId = 5, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 9, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 3, LessonHourId = 6, SubjectId = 5, TeacherId = GetTeacherId(5), ClassroomId = 17, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 3, LessonHourId = 7, SubjectId = 20, TeacherId = GetTeacherId(20), ClassroomId = 14, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });

            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 4, LessonHourId = 1, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 6, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 4, LessonHourId = 2, SubjectId = 8, TeacherId = GetTeacherId(8), ClassroomId = 8, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 4, LessonHourId = 3, SubjectId = 9, TeacherId = GetTeacherId(9), ClassroomId = 8, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 4, LessonHourId = 4, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 9, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 4, LessonHourId = 5, SubjectId = 3, TeacherId = GetTeacherId(3), ClassroomId = 10, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 4, LessonHourId = 6, SubjectId = 11, TeacherId = GetTeacherId(11), ClassroomId = 12, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 4, LessonHourId = 7, SubjectId = 5, TeacherId = GetTeacherId(5), ClassroomId = 17, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });

            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 5, LessonHourId = 1, SubjectId = 7, TeacherId = GetTeacherId(7), ClassroomId = 13, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 5, LessonHourId = 2, SubjectId = 6, TeacherId = GetTeacherId(6), ClassroomId = 11, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 5, LessonHourId = 3, SubjectId = 2, TeacherId = GetTeacherId(2), ClassroomId = 9, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 5, LessonHourId = 4, SubjectId = 1, TeacherId = GetTeacherId(1), ClassroomId = 6, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 5, LessonHourId = 5, SubjectId = 10, TeacherId = GetTeacherId(10), ClassroomId = 7, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });
            schedules.Add(new WeeklySchedule { ClassId = 2, DayOfWeek = 5, LessonHourId = 6, SubjectId = 3, TeacherId = GetTeacherId(3), ClassroomId = 10, SchoolYearId = 1, SemesterId = 1, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null });

            _context.WeeklySchedules.AddRange(schedules);
            _context.SaveChanges();
        }

        private void SeedOperationalData()
        {
            var teachersData = GetTeachersData();
            var subjectToEmail = teachersData.ToDictionary(t => t.SubjectId, t => t.Email);
            var gradeTypeIds = _context.GradeTypes.Select(x => x.Id).ToList();
            var gradeCategoryIds = _context.GradeCategories.Select(x => x.Id).ToList();

            if (!gradeTypeIds.Any() || !gradeCategoryIds.Any())
                throw new Exception("Brak typów lub kategorii ocen");

            var weeklySchedules = _context.WeeklySchedules.ToList();
            if (!weeklySchedules.Any()) return;

            var today = DateTime.Now.Date;
            var startDate = today.AddDays(-30);
            var generatedLessons = new List<Lesson>();

            for (var date = startDate; date <= today; date = date.AddDays(1))
            {
                int dayOfWeek = (int)date.DayOfWeek;
                if (dayOfWeek == 0 || dayOfWeek == 6) continue;

                var dailySchedules = weeklySchedules.Where(ws => ws.DayOfWeek == dayOfWeek).ToList();
                foreach (var slot in dailySchedules)
                {
                    generatedLessons.Add(new Lesson
                    {
                        ClassId = slot.ClassId,
                        SubjectId = slot.SubjectId,
                        TeacherId = slot.TeacherId,
                        ClassroomId = slot.ClassroomId,
                        LessonHourId = slot.LessonHourId,
                        Topic = $"Temat lekcji",
                        StatusId = 1,
                        IsActive = true,
                        Date = date,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        ModifiedByUserId = slot.TeacherId
                    });
                }
            }

            _context.Lessons.AddRange(generatedLessons);
            _context.SaveChanges();

            var attendances = new List<Attendance>();
            var grades = new List<Grade>();
            var attendanceCheck = new HashSet<(int StudentId, int LessonId)>();
            var classIds = new[] { 1, 2 };

            foreach (var classId in classIds)
            {
                var studentIds = _context.ClassStudents
                    .Where(cs => cs.ClassId == classId)
                    .Select(cs => cs.StudentId)
                    .ToList();

                var lessonsBySubject = generatedLessons
                    .Where(l => l.ClassId == classId)
                    .GroupBy(l => l.SubjectId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var studentId in studentIds)
                {
                    foreach (var subjectEntry in lessonsBySubject)
                    {
                        var subjectId = subjectEntry.Key;
                        var subjectLessons = subjectEntry.Value;
                        int targetCount = _random.Next(2, 4);
                        int countToTake = Math.Min(targetCount, subjectLessons.Count);

                        var selectedForGrades = subjectLessons
                            .OrderBy(x => _random.Next())
                            .Take(countToTake)
                            .ToList();

                        foreach (var lesson in selectedForGrades)
                        {
                            grades.Add(new Grade
                            {
                                StudentId = studentId,
                                TeacherId = lesson.TeacherId,
                                SubjectId = subjectId,
                                GradeTypeId = gradeTypeIds[_random.Next(gradeTypeIds.Count)],
                                GradeCategoryId = gradeCategoryIds[_random.Next(gradeCategoryIds.Count)],
                                DateTime = lesson.Date,
                                Comment = null,
                                IsActive = true,
                                CreatedAt = lesson.Date,
                                UpdatedAt = lesson.Date,
                                ModifiedByUserId = lesson.TeacherId
                            });

                            attendances.Add(new Attendance
                            {
                                StudentId = studentId,
                                LessonId = lesson.Id,
                                AttendanceTypeId = 1,
                                IsActive = true,
                                CreatedAt = lesson.Date,
                                UpdatedAt = lesson.Date,
                                ModifiedByUserId = lesson.TeacherId
                            });

                            attendanceCheck.Add((studentId, lesson.Id));
                        }
                    }

                    var allClassLessons = generatedLessons.Where(l => l.ClassId == classId);

                    foreach (var lesson in allClassLessons)
                    {
                        if (attendanceCheck.Contains((studentId, lesson.Id))) continue;

                        int roll = _random.Next(0, 100);
                        int typeId;

                        if (roll < 85) typeId = 1;
                        else if (roll < 95) typeId = 3;
                        else typeId = 2;

                        attendances.Add(new Attendance
                        {
                            StudentId = studentId,
                            LessonId = lesson.Id,
                            AttendanceTypeId = typeId,
                            IsActive = true,
                            CreatedAt = lesson.Date,
                            UpdatedAt = lesson.Date,
                            ModifiedByUserId = lesson.TeacherId
                        });
                    }
                }
            }

            _context.Attendances.AddRange(attendances);
            _context.Grades.AddRange(grades);
            _context.SaveChanges();
        }

        private void SeedTickets()
        {
            var reasons = _context.TicketReasons.ToList();
            if (!reasons.Any()) return;

            var adminId = _context.Users.First(u => u.Email == adminMail).Id;

            var tickets = new List<Ticket>
            {
                new Ticket
                {
                    Email = "murbaniak@szkola.edu.pl",
                    ReasonId = reasons.First(r => r.Name == "Problem z logowaniem").Id,
                    Content = "Nie mogę się zalogować do systemu. Wprowadziłem poprawne dane, ale wywietla się komunikat o blednym haśle.",
                    IsClosed = true,
                    ClosedAt = DateTime.Now.AddDays(-1),
                    AdminResponse = "<p>Hasło zostało zresetowane. Proszę sprawdzić, czy problem z logowaniem został rozwiązany, a po udanej próbie logowania natychmiast zmienić hasło.</p><p>Nowe hasło: E4G$%Vedv%3D3Ad.</p>",
                    ModifiedByUserId = adminId,
                    CreatedAt = DateTime.Now.AddDays(-5),
                    UpdatedAt = DateTime.Now.AddDays(-1)
                },
                new Ticket
                {
                    Email = "abaran@szkola.edu.pl",
                    ReasonId = reasons.First(r => r.Name == "Problem z logowaniem").Id,
                    Content = "Próbuję zalogować się do systemu, ale ciągle pokazuje się błąd. Czy moje konto jest aktywne?",
                    IsClosed = false,
                    CreatedAt = DateTime.Now.AddHours(-3),
                    UpdatedAt = DateTime.Now.AddHours(-3),
                    ModifiedByUserId = null
                },
                new Ticket
                {
                    Email = "mkrajewska@szkola.edu.pl",
                    ReasonId = reasons.First(r => r.Name == "Problem z logowaniem").Id,
                    Content = "Zapomniałem hasła, a email z linkiem do resetowania nie przychodzi.",
                    IsClosed = false,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    UpdatedAt = DateTime.Now.AddDays(-1),
                    ModifiedByUserId = null
                },
                new Ticket
                {
                    Email = "pmroz@szkola.edu.pl",
                    ReasonId = reasons.First(r => r.Name == "Zmiana danych osobowych").Id,
                    Content = "Proszę o zmianę mojego numeru telefonu w systemie. Nowy numer: 600-700-800",
                    IsClosed = true,
                    ClosedAt = DateTime.Now.AddDays(-2),
                    AdminResponse = "<p>Numer telefonu został zaktualizowany w systemie.</p>",
                    ModifiedByUserId = adminId,
                    CreatedAt = DateTime.Now.AddDays(-3),
                    UpdatedAt = DateTime.Now.AddDays(-2)
                },
                new Ticket
                {
                    Email = "awroblewska@szkola.edu.pl",
                    ReasonId = reasons.First(r => r.Name == "Zmiana danych osobowych").Id,
                    Content = "W systemie jest błędny adres zamieszkania mojego dziecka. Jak mogę to zmienić?",
                    IsClosed = false,
                    CreatedAt = DateTime.Now.AddHours(-12),
                    UpdatedAt = DateTime.Now.AddHours(-12),
                    ModifiedByUserId = null
                },
                new Ticket
                {
                    Email = teacherMail,
                    ReasonId = reasons.First(r => r.Name == "Zmiana danych osobowych").Id,
                    Content = "Proszę o aktualizację adresu zamieszkania - nowy adres to: ul. Nowa 14, 44-200 Rybnik",
                    IsClosed = false,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    UpdatedAt = DateTime.Now.AddHours(-5),
                    ModifiedByUserId = null
                },
                new Ticket
                {
                    Email = "esikora@szkola.edu.pl",
                    ReasonId = reasons.First(r => r.Name == "Błąd w systemie").Id,
                    Content = "Przy próbie usprawiedliwienia nieobecności mojej córki system pokazuje błąd 404.",
                    IsClosed = false,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    UpdatedAt = DateTime.Now.AddDays(-2),
                    ModifiedByUserId = null
                },
                new Ticket
                {
                    Email = "mazakrzewska@szkola.edu.pl",
                    ReasonId = reasons.First(r => r.Name == "Błąd w systemie").Id,
                    Content = "Plan lekcji nie wyświetla się poprawnie - brakuje przedmiotów z piątku.",
                    IsClosed = true,
                    ClosedAt = DateTime.Now.AddHours(-2),
                    AdminResponse = "<p>Problem został naprawiony - plan lekcji powinien wyświetlać się teraz poprawnie.</p><p>Dziękujemy za zgłoszenie.</p>",
                    ModifiedByUserId = adminId,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    UpdatedAt = DateTime.Now.AddHours(-2)
                },
                new Ticket
                {
                    Email = "plaskowski@szkola.edu.pl",
                    ReasonId = reasons.First(r => r.Name == "Błąd w systemie").Id,
                    Content = "Eksport planu lekcji do pliku CSV nie działa - dostaję błąd o kodzie 500.",
                    IsClosed = false,
                    CreatedAt = DateTime.Now.AddHours(-8),
                    UpdatedAt = DateTime.Now.AddHours(-8),
                    ModifiedByUserId = null
                },
                new Ticket
                {
                    Email = "kglowacki@szkola.edu.pl",
                    ReasonId = reasons.First(r => r.Name == "Inne").Id,
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    IsClosed = true,
                    ClosedAt = DateTime.Now.AddDays(-1),
                    AdminResponse = "<p>Odpowiedź lorem ipsum</p>",
                    ModifiedByUserId = adminId,
                    CreatedAt = DateTime.Now.AddDays(-4),
                    UpdatedAt = DateTime.Now.AddDays(-1),
                },
                new Ticket
                {
                    Email = "zmakowska@szkola.edu.pl",
                    ReasonId = reasons.First(r => r.Name == "Inne").Id,
                    Content = "Czy możliwe jest dodanie powiadomień SMS o nowych ocenach?",
                    IsClosed = false,
                    CreatedAt = DateTime.Now.AddDays(-3),
                    UpdatedAt = DateTime.Now.AddDays(-3),
                    ModifiedByUserId = null
                }
            };

            _context.Tickets.AddRange(tickets);
            _context.SaveChanges();
        }

        private void SeedAnnouncements()
        {
            var teacherIds = _context.UserRoles
                .Include(ur => ur.Role)
                .Where(ur => ur.Role.Level == 2)
                .Select(ur => ur.UserId)
                .ToList();

            if (!teacherIds.Any()) return;

            var announcementsData = new[]
            {
                new {
                    Title = "Zebranie rodziców - klasy 1-3",
                    Description = "<p><strong>Szanowni Rodzice,</strong></p><p>Zapraszamy na zebranie, które odbędzie się:</p><ul><li>Data: <strong>15 lutego 2026</strong></li><li>Godzina: <strong>18:00</strong></li><li>Miejsce: aula</li></ul><p>Podczas spotkania omówimy:</p><ol><li>Postępy w nauce uczniów</li><li>Dalsze plany nauki</li><li>Proponowane wycieczki szkolne</li></ol><p>Obecność obowiązkowa!</p>"
                },
                new {
                    Title = "Ważna informacja - zmiana godzin pracy sekretariatu",
                    Description = "<p>Informujemy, że od <strong>1 lutego 2026</strong> sekretariat szkoły będzie czynny w następujących godzinach:</p><ul><li>Poniedziałek-Piątek: <strong>7:30 - 15:30</strong></li><li>Przerwa: <strong>12:00 - 12:30</strong></li></ul><p>W sprawach pilnych prosimy o kontakt telefoniczny pod numerem <strong>22 123 45 67</strong>.</p><p>Dziękujemy za zrozumienie.</p>"
                },
                new {
                    Title = "Zmiana w planie lekcji - klasa 1A",
                    Description = "<p><strong>Uwaga!</strong> W dniu <em>jutrzejszym (21.01.2026)</em> nastąpi zmiana w planie lekcji klasy 1A:</p><table><thead><tr><th>Lekcja</th><th>Było</th><th>Będzie</th></tr></thead><tbody><tr><td>3</td><td>Matematyka</td><td><strong>Wychowanie fizyczne</strong></td></tr><tr><td>4</td><td>Wychowanie fizyczne</td><td><strong>Matematyka</strong></td></tr></tbody></table><p>Prosimy o zabranie odpowiedniego stroju sportowego.</p>"
                },
                new {
                    Title = "Konkursy szkolne - zapisy do 30 stycznia",
                    Description = "<p>Zapraszamy uczniów do udziału w <strong>konkursach szkolnych</strong>:</p><h3>Konkurs matematyczny</h3><ul><li>Termin: 10 lutego 2026</li><li>Klasy: 4-8</li><li>Zgłoszenia: u wychowawcy klasy</li></ul><h3>Konkurs plastyczny \"Moja szkoła\"</h3><ul><li>Termin oddania prac: 15 lutego 2026</li><li>Klasy: 1-8</li><li>Format: A3, dowolna technika</li></ul><p><em>Zachęcamy do aktywnego udziału!</em> Laureaci otrzymają dyplomy i nagrody rzeczowe.</p>"
                },
                new {
                    Title = "Wycieczka do Krakowa - klasy 7-8",
                    Description = "<p><strong>Organizujemy wycieczkę do Krakowa</strong> dla uczniów klas 7-8!</p><h3>Szczegóły wycieczki:</h3><ul><li>Termin: <strong>5-7 marca 2026</strong> (3 dni)</li><li>Koszt: <strong>450 zł/osobę</strong> (transport, nocleg, wyżywienie, bilety wstępu)</li><li>Program: Wawel, Kopalnię Soli, Muzeum Fabryki Schindlera</li></ul><h3>Terminy:</h3><ol><li>Wpłata zaliczki 150 zł: <em>do 10 lutego</em></li><li>Dopłata 300 zł: <em>do 25 lutego</em></li><li>Oddanie zgód: <em>do 1 marca</em></li></ol><p>Liczba miejsc ograniczona do 50 osób. <strong>Decyduje kolejność zgłoszeń!</strong></p>"
                }
            };

            var announcements = new List<Announcement>();

            for (int i = 0; i < announcementsData.Length; i++)
            {
                var daysBack = i * 2;
                var date = DateTime.Now.AddDays(-daysBack);
                var authorId = teacherIds[i % teacherIds.Count];

                announcements.Add(new Announcement
                {
                    Title = announcementsData[i].Title,
                    Description = announcementsData[i].Description,
                    CreatedAt = date,
                    UpdatedAt = date,
                    AuthorId = authorId,
                    IsActive = true,
                    ModifiedByUserId = authorId
                });
            }

            announcements = announcements.OrderBy(a => a.CreatedAt).ToList();

            _context.Announcements.AddRange(announcements);
            _context.SaveChanges();

            foreach (var announcement in announcements)
            {
                _context.AnnouncementTargets.Add(new AnnouncementTarget
                {
                    AnnouncementId = announcement.Id,
                    RoleId = null
                });
            }
            _context.SaveChanges();
        }

        private void SeedClassrooms()
        {
            var classrooms = new List<Classroom>
            {
                new Classroom { Name = "101", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "102", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "103", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "104", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "105", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "201", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "202", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "203", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "204", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "205", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "301", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "302", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "303", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "304", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "305", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "gimnastyczna 1", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "gimnastyczna 2", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Classroom { Name = "aula", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null }
            };

            _context.Classrooms.AddRange(classrooms);
            _context.SaveChanges();
        }

        private void SeedSubjects()
        {
            var subjects = new List<Subject>
            {
                new Subject { Name = "matematyka", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "język polski", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "język angielski", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "język niemiecki", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "informatyka", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "wychowanie fizyczne", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "historia", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "wiedza o spoeczeństwie", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "biologia", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "chemia", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "fizyka", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "geografia", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "przyroda", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "plastyka", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "muzyka", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "zajęcia artystyczne", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "religia", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "etyka", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "wychowanie do życia w rodzinie", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "technika", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Subject { Name = "edukacja dla bezpieczeństwa", IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null }
            };

            _context.Subjects.AddRange(subjects);
            _context.SaveChanges();
        }

        private void SeedBaseClasses()
        {
            var schoolYear = _context.SchoolYears.FirstOrDefault(sy => sy.IsActive);
            if (schoolYear == null) return;

            var classes = new List<Class>
            {
                new Class { Level = 1, Letter = "A", SchoolYearId = schoolYear.Id, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null },
                new Class { Level = 8, Letter = "C", SchoolYearId = schoolYear.Id, IsActive = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, ModifiedByUserId = null }
            };

            _context.Classes.AddRange(classes);
            _context.SaveChanges();
        }
    }
}
