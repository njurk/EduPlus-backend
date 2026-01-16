using Data.Data.CMS;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public static class ModelSeeder
    {
        private static readonly DateTime InitialDateTime = new(2025, 12, 27, 10, 0, 0, DateTimeKind.Utc);

        public static void Seed(ModelBuilder modelBuilder)
        {
            SeedTargets(modelBuilder);
            SeedPages(modelBuilder);
            SeedPageContents(modelBuilder);
            SeedRoles(modelBuilder);
            SeedSchoolYears(modelBuilder);
            SeedAttendanceTypes(modelBuilder);
            SeedGradeTypes(modelBuilder);
            SeedGradeCategories(modelBuilder);
            SeedLessonStatuses(modelBuilder);
            SeedLessonHours(modelBuilder);
            SeedClassrooms(modelBuilder);
            SeedSubjects(modelBuilder);
            SeedSemesters(modelBuilder);
            SeedClasses(modelBuilder);
        }

        private static void SeedTargets(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Target>().HasData(
                new Target { Id = 1, Label = "WebAdmin", Title = "Administrator - strona internetowa", CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Target { Id = 2, Label = "WebTeacher", Title = "Nauczyciel - strona internetowa", CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Target { Id = 3, Label = "MobileParent", Title = "Rodzic - aplikacja mobilna", CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Target { Id = 4, Label = "MobileStudent", Title = "Uczeń - aplikacja mobilna", CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Target { Id = 5, Label = "All", Title = "Wszystkie platformy", CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime }
            );
        }

        private static void SeedPages(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Page>().HasData(
                new Page { Id = 1, Title = "Ustawienia systemu", Link = "system", Position = 1, TargetId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Page { Id = 2, Title = "Dashboard", Link = "dashboard", Position = 1, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Page { Id = 3, Title = "Uzytkownicy", Link = "users", Position = 2, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Page { Id = 4, Title = "Ogloszenia", Link = "announcements", Position = 3, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Page { Id = 5, Title = "Zgloszenia", Link = "tickets", Position = 4, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Page { Id = 6, Title = "Usprawiedliwienia", Link = "excuses", Position = 5, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Page { Id = 7, Title = "Lekcje", Link = "lessons", Position = 6, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Page { Id = 8, Title = "Plan lekcji", Link = "schedule", Position = 7, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Page { Id = 9, Title = "Oceny", Link = "grades", Position = 8, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Page { Id = 10, Title = "Frekwencja", Link = "attendance", Position = 9, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Page { Id = 11, Title = "Zarzadzanie klasami", Link = "classManagement", Position = 10, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Page { Id = 12, Title = "Ustawienia konta", Link = "settings", Position = 11, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Page { Id = 13, Title = "Konfiguracja systemu", Link = "systemConfig", Position = 12, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Page { Id = 14, Title = "Pasek boczny", Link = "layout", Position = 0, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Page { Id = 15, Title = "Login", Link = "login", Position = 0, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime }
            );
        }

        private static void SeedPageContents(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PageContent>().HasData(
                new PageContent { Id = 1, Key = "systemName", Value = "EduPlus", PageId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 2, Key = "systemLogo", Value = "/assets/logo.svg", PageId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },

                new PageContent { Id = 3, Key = "title", Value = "Dashboard", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 4, Key = "loading", Value = "Ladowanie...", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },

                new PageContent { Id = 5, Key = "title", Value = "Uzytkownicy", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 6, Key = "actions.add", Value = "Dodaj", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 7, Key = "actions.save", Value = "Zapisz", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 8, Key = "actions.cancel", Value = "Anuluj", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 9, Key = "loading", Value = "Ladowanie...", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 10, Key = "empty", Value = "Brak danych", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 11, Key = "modal.add", Value = "Dodaj uzytkownika", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 12, Key = "modal.edit", Value = "Edytuj uzytkownika", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 13, Key = "columns.name", Value = "Imie i nazwisko", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 14, Key = "columns.email", Value = "Email", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 15, Key = "columns.roles", Value = "Role", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 16, Key = "columns.createdAt", Value = "Utworzono", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 17, Key = "columns.updatedAt", Value = "Edytowano", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 18, Key = "columns.actions", Value = "Akcje", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 19, Key = "sort.name", Value = "Nazwisko", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 20, Key = "sort.email", Value = "Email", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 21, Key = "sort.updatedAt", Value = "Edytowano", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 22, Key = "confirm.delete", Value = "Czy na pewno chcesz usunac?", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 23, Key = "confirm.restore", Value = "Czy na pewno chcesz przywrocic?", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },

                new PageContent { Id = 24, Key = "title", Value = "Ogloszenia", PageId = 4, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 25, Key = "actions.add", Value = "Dodaj", PageId = 4, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 26, Key = "loading", Value = "Ladowanie...", PageId = 4, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 27, Key = "empty", Value = "Brak ogloszen", PageId = 4, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },

                new PageContent { Id = 28, Key = "title", Value = "Zgloszenia", PageId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 29, Key = "loading", Value = "Ladowanie...", PageId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 30, Key = "empty", Value = "Brak zgloszen", PageId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },

                new PageContent { Id = 31, Key = "title", Value = "Usprawiedliwienia", PageId = 6, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 32, Key = "loading", Value = "Ladowanie...", PageId = 6, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 33, Key = "empty", Value = "Brak usprawiedliwien", PageId = 6, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },

                new PageContent { Id = 34, Key = "title", Value = "Lekcje", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 35, Key = "loading", Value = "Ladowanie...", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 36, Key = "empty", Value = "Brak lekcji", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },

                new PageContent { Id = 37, Key = "title", Value = "Plan lekcji", PageId = 8, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 38, Key = "loading", Value = "Ladowanie planu...", PageId = 8, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },

                new PageContent { Id = 39, Key = "title", Value = "Oceny", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 40, Key = "actions.add", Value = "Dodaj", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 41, Key = "loading", Value = "Ladowanie...", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 42, Key = "empty", Value = "Brak ocen", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 43, Key = "columns.student", Value = "Uczen", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 44, Key = "columns.class", Value = "Klasa", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 45, Key = "columns.subject", Value = "Przedmiot", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 46, Key = "columns.grade", Value = "Ocena", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 47, Key = "columns.category", Value = "Kategoria", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 48, Key = "columns.date", Value = "Data", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 49, Key = "sort.student", Value = "Uczen", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 50, Key = "sort.subject", Value = "Przedmiot", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 51, Key = "sort.date", Value = "Data", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },

                new PageContent { Id = 52, Key = "title", Value = "Frekwencja", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 53, Key = "actions.add", Value = "Dodaj", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 54, Key = "loading", Value = "Ladowanie...", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 55, Key = "empty", Value = "Brak danych frekwencji", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 56, Key = "sort.student", Value = "Uczen", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 57, Key = "sort.date", Value = "Data", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 58, Key = "columns.student", Value = "Uczen", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 59, Key = "columns.lesson", Value = "Lekcja", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 60, Key = "columns.status", Value = "Status", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 61, Key = "columns.date", Value = "Data", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },

                new PageContent { Id = 62, Key = "title", Value = "Zarzadzanie klasami", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 63, Key = "actions.add", Value = "Dodaj", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 64, Key = "actions.save", Value = "Zapisz", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 65, Key = "actions.cancel", Value = "Anuluj", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 66, Key = "actions.assign", Value = "Przypisz", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 67, Key = "loading", Value = "Ladowanie...", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 68, Key = "empty", Value = "Brak klas", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 69, Key = "empty.students", Value = "Brak uczniow", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 70, Key = "empty.subjects", Value = "Brak przedmiotow", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 71, Key = "modal.newClass", Value = "Nowa klasa", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 72, Key = "modal.editClass", Value = "Edycja klasy", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 73, Key = "modal.assignStudents", Value = "Przypisz uczniow", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 74, Key = "modal.assignSubject", Value = "Przypisz przedmiot", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 75, Key = "tabs.students", Value = "Uczniowie", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 76, Key = "tabs.subjects", Value = "Przedmioty", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 77, Key = "form.level", Value = "Poziom", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 78, Key = "form.section", Value = "Oddzial", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 79, Key = "form.subject", Value = "Przedmiot", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 80, Key = "form.teacher", Value = "Nauczyciel", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 81, Key = "form.selectOption", Value = "Wybierz...", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 82, Key = "form.selectTeacher", Value = "Wybierz nauczyciela...", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 83, Key = "form.selectSubjectFirst", Value = "Najpierw wybierz przedmiot", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 84, Key = "placeholder.search", Value = "Szukaj...", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 85, Key = "columns.class", Value = "Klasa", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 86, Key = "columns.studentCount", Value = "Liczba uczniow", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 87, Key = "columns.ordinal", Value = "Lp.", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 88, Key = "columns.student", Value = "Uczen", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 89, Key = "columns.email", Value = "Email", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 90, Key = "columns.subject", Value = "Przedmiot", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 91, Key = "columns.teacher", Value = "Nauczyciel", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 92, Key = "columns.createdAt", Value = "Utworzono", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 93, Key = "columns.updatedAt", Value = "Edytowano", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 94, Key = "columns.actions", Value = "Akcje", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 95, Key = "sort.class", Value = "Klasa", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 96, Key = "sort.lastName", Value = "Nazwisko", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 97, Key = "sort.email", Value = "Email", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 98, Key = "sort.ordinal", Value = "Lp.", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 99, Key = "sort.subject", Value = "Przedmiot", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 100, Key = "sort.teacher", Value = "Nauczyciel", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 101, Key = "sort.createdAt", Value = "Utworzono", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 102, Key = "sort.updatedAt", Value = "Edytowano", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 103, Key = "confirm.delete", Value = "Usunac?", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 104, Key = "confirm.restore", Value = "Przywrocic?", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 105, Key = "confirm.removeStudent", Value = "Czy na pewno chcesz usunac tego ucznia z klasy?", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 106, Key = "confirm.removeSubject", Value = "Czy na pewno chcesz usunac ten przedmiot z klasy?", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 107, Key = "error.general", Value = "Wystapil blad", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 108, Key = "error.save", Value = "Blad zapisu", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 109, Key = "error.delete", Value = "Blad usuwania", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 110, Key = "error.noTeachers", Value = "Brak nauczycieli przypisanych do tego przedmiotu.", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },

                new PageContent { Id = 111, Key = "title", Value = "Ustawienia konta", PageId = 12, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 112, Key = "section.profile", Value = "Profil", PageId = 12, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 113, Key = "section.password", Value = "Zmiana hasla", PageId = 12, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },

                new PageContent { Id = 114, Key = "modal.new", Value = "Nowy element", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 115, Key = "modal.edit", Value = "Edycja elementu", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 116, Key = "actions.save", Value = "Zapisz", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 117, Key = "actions.cancel", Value = "Anuluj", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 118, Key = "confirm.restore", Value = "Przywrocic element?", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 119, Key = "confirm.moveToTrash", Value = "Przeniesc do kosza?", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 120, Key = "confirm.permanentDelete", Value = "Usunac trwale?", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 121, Key = "error.save", Value = "Blad zapisu", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 122, Key = "error.status", Value = "Blad zmiany statusu", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 123, Key = "error.delete", Value = "Blad usuwania", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 124, Key = "columns.number", Value = "Nr", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 125, Key = "columns.hours", Value = "Godziny", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 126, Key = "columns.symbol", Value = "Symbol", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 127, Key = "columns.name", Value = "Nazwa", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 128, Key = "columns.value", Value = "Wartosc", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 129, Key = "columns.weight", Value = "Waga", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 130, Key = "columns.shortCode", Value = "Skrot", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 131, Key = "columns.createdAt", Value = "Utworzono", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 132, Key = "columns.updatedAt", Value = "Edytowano", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 133, Key = "columns.actions", Value = "Akcje", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },

                new PageContent { Id = 134, Key = "systemName", Value = "EduPlus", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 135, Key = "version", Value = "v1.0.0 EduPlus", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 136, Key = "confirm.logout", Value = "Na pewno chcesz sie wylogowac?", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 137, Key = "greeting.prefix", Value = "Witaj, ", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 138, Key = "greeting.suffix", Value = "!", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 139, Key = "greeting.defaultUser", Value = "Uzytkownik", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 140, Key = "nav.dashboard", Value = "Pulpit", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 141, Key = "nav.section.management", Value = "Zarzadzanie", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 142, Key = "nav.users", Value = "Uzytkownicy", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 143, Key = "nav.classes", Value = "Klasy", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 144, Key = "nav.announcements", Value = "Ogloszenia", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 145, Key = "nav.tickets", Value = "Zgloszenia", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 146, Key = "nav.section.teaching", Value = "Nauczanie", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 147, Key = "nav.schedule", Value = "Plany lekcji", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 148, Key = "nav.lessons", Value = "Lekcje", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 149, Key = "nav.grades", Value = "Oceny", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 150, Key = "nav.attendance", Value = "Frekwencja", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 151, Key = "nav.excuses", Value = "Usprawiedliwienia", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 152, Key = "nav.section.system", Value = "System", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 153, Key = "nav.config", Value = "Konfiguracja", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 154, Key = "nav.cms", Value = "CMS", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 155, Key = "nav.settings", Value = "Ustawienia", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 156, Key = "nav.logout", Value = "Wyloguj", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },

                new PageContent { Id = 157, Key = "title", Value = "EduPlus Admin", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 158, Key = "logoUrl", Value = "/logo-512.png", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 159, Key = "logoAlt", Value = "EduPlus Logo", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 160, Key = "form.email", Value = "Email", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 161, Key = "form.password", Value = "Haslo", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 162, Key = "form.forgotPassword", Value = "Zapomniałes hasla?", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 163, Key = "button.login", Value = "Zaloguj sie", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 164, Key = "button.loading", Value = "Logowanie...", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 165, Key = "error.login", Value = "Wystapil blad logowania", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new PageContent { Id = 166, Key = "footer", Value = "EduPlus v1.0.0", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime }
            );
        }

        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Administrator", IsActive = true, Description = "Najwyższy poziom uprawnień, dostęp do wszystkiego", Level = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Role { Id = 2, Name = "Nauczyciel", IsActive = true, Description = "Zarządzanie przydzielonymi zasobami", Level = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Role { Id = 3, Name = "Rodzic", IsActive = true, Description = "Przeglądanie danych przypisanego użytkownika, możliwość usprawiedliwienia", Level = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Role { Id = 4, Name = "Uczeń", IsActive = true, Description = "Przeglądanie własnych danych", Level = 4, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime }
            );
        }

        private static void SeedSchoolYears(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchoolYear>().HasData(
                new SchoolYear { Id = 1, Name = "2025/2026", StartDate = new DateOnly(2025, 9, 1), EndDate = new DateOnly(2026, 6, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new SchoolYear { Id = 2, Name = "2026/2027", StartDate = new DateOnly(2026, 9, 1), EndDate = new DateOnly(2027, 6, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime }
            );
        }
        
        private static void SeedAttendanceTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendanceType>().HasData(
                new AttendanceType { Id = 1, Name = "Obecność", ShortCode = "OB", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new AttendanceType { Id = 2, Name = "Nieobecność", ShortCode = "NB", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new AttendanceType { Id = 3, Name = "Spóźnienie", ShortCode = "SP", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new AttendanceType { Id = 4, Name = "Usprawiedliwione", ShortCode = "U", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new AttendanceType { Id = 5, Name = "Zwolnienie", ShortCode = "ZW", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime }
            );
        }
        
        private static void SeedGradeTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GradeType>().HasData(
                new GradeType { Id = 1, Numeric = "1", Name = "Niedostateczny", Value = 1.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new GradeType { Id = 2, Numeric = "2", Name = "Dopuszczający", Value = 2.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new GradeType { Id = 3, Numeric = "3", Name = "Dostateczny", Value = 3.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new GradeType { Id = 4, Numeric = "4", Name = "Dobry", Value = 4.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new GradeType { Id = 5, Numeric = "5", Name = "Bardzo dobry", Value = 5.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new GradeType { Id = 6, Numeric = "6", Name = "Celujący", Value = 6.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime }
            );
        }
        
        private static void SeedGradeCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GradeCategory>().HasData(
                new GradeCategory { Id = 1, Name = "Sprawdzian", Weight = 3, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new GradeCategory { Id = 2, Name = "Kartkówka", Weight = 2, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new GradeCategory { Id = 3, Name = "Odpowiedź ustna", Weight = 1, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new GradeCategory { Id = 4, Name = "Aktywność", Weight = 1, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new GradeCategory { Id = 5, Name = "Zadanie domowe", Weight = 1, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime }
            );
        }
        
        private static void SeedLessonStatuses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LessonStatus>().HasData(
                new LessonStatus { Id = 1, Name = "Zaplanowana", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new LessonStatus { Id = 2, Name = "Zrealizowana", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new LessonStatus { Id = 3, Name = "Odwołana", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime }
            );
        }
        
        private static void SeedLessonHours(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LessonHour>().HasData(
                new LessonHour { Id = 1, OrderNumber = 1, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(8, 45), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new LessonHour { Id = 2, OrderNumber = 2, StartTime = new TimeOnly(8, 55), EndTime = new TimeOnly(9, 40), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new LessonHour { Id = 3, OrderNumber = 3, StartTime = new TimeOnly(9, 50), EndTime = new TimeOnly(10, 35), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new LessonHour { Id = 4, OrderNumber = 4, StartTime = new TimeOnly(10, 45), EndTime = new TimeOnly(11, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new LessonHour { Id = 5, OrderNumber = 5, StartTime = new TimeOnly(11, 45), EndTime = new TimeOnly(12, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new LessonHour { Id = 6, OrderNumber = 6, StartTime = new TimeOnly(12, 50), EndTime = new TimeOnly(13, 35), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new LessonHour { Id = 7, OrderNumber = 7, StartTime = new TimeOnly(13, 45), EndTime = new TimeOnly(14, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new LessonHour { Id = 8, OrderNumber = 8, StartTime = new TimeOnly(14, 40), EndTime = new TimeOnly(15, 25), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new LessonHour { Id = 9, OrderNumber = 9, StartTime = new TimeOnly(15, 30), EndTime = new TimeOnly(16, 15), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime }
            );
        }
        
        private static void SeedClassrooms(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Classroom>().HasData(
                new Classroom { Id = 1, Name = "101", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 2, Name = "102", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 3, Name = "103", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 4, Name = "104", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 5, Name = "105", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 6, Name = "201", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 7, Name = "202", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 8, Name = "203", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 9, Name = "204", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 10, Name = "205", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 11, Name = "301", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 12, Name = "302", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 13, Name = "303", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 14, Name = "304", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 15, Name = "305", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 16, Name = "gimnastyczna 1", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 17, Name = "gimnastyczna 2", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Classroom { Id = 18, Name = "aula", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime }
            );
        }
        
        private static void SeedSubjects(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>().HasData(
                new Subject { Id = 1, Name = "matematyka", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 2, Name = "język polski", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 3, Name = "język angielski", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 4, Name = "język niemiecki", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 5, Name = "informatyka", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 6, Name = "wychowanie fizyczne", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 7, Name = "historia", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 8, Name = "WOS", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 9, Name = "biologia", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 10, Name = "chemia", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 11, Name = "fizyka", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 12, Name = "Geografia", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 13, Name = "przyroda", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 14, Name = "plastyka", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 15, Name = "muzyka", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 16, Name = "zajęcia artystyczne", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 17, Name = "religia", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 18, Name = "etyka", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 19, Name = "WDŻ", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 20, Name = "technika", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Subject { Id = 21, Name = "EDB", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime }
            );
        }
        
        private static void SeedSemesters(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Semester>().HasData(
                new Semester { Id = 1, SchoolYearId = 1, Name = "Semestr 1", StartDate = new DateOnly(2025, 09, 01), EndDate = new DateOnly(2026, 01, 31), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Semester { Id = 2, SchoolYearId = 1, Name = "Semestr 2", StartDate = new DateOnly(2026, 02, 01), EndDate = new DateOnly(2026, 06, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Semester { Id = 3, SchoolYearId = 2, Name = "Semestr 1", StartDate = new DateOnly(2026, 09, 01), EndDate = new DateOnly(2027, 01, 31), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Semester { Id = 4, SchoolYearId = 2, Name = "Semestr 2", StartDate = new DateOnly(2027, 02, 01), EndDate = new DateOnly(2027, 06, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime }
            );
        }
        
        private static void SeedClasses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>().HasData(
                new Class { Id = 1, Level = 1, Letter = "A", SchoolYearId = 1, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new Class { Id = 2, Level = 8, Letter = "C", SchoolYearId = 1, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime }
            );
        }
    }
}
