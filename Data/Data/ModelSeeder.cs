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
            SeedSemesters(modelBuilder);
            SeedTicketReasons(modelBuilder);
        }

        private static void SeedTargets(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Target>().HasData(
                new Target { Id = 1, Label = "WebAdmin", Title = "Administrator - strona internetowa", CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Target { Id = 2, Label = "WebTeacher", Title = "Nauczyciel - strona internetowa", CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Target { Id = 3, Label = "MobileParent", Title = "Rodzic - aplikacja mobilna", CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Target { Id = 4, Label = "MobileStudent", Title = "Uczeń - aplikacja mobilna", CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Target { Id = 5, Label = "All", Title = "System", CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );
        }

        private static void SeedPages(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Page>().HasData(
                new Page { Id = 1, Title = "System", Link = "system", Position = 1, TargetId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 2, Title = "Dashboard", Link = "dashboard", Position = 1, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 3, Title = "Użytkownicy", Link = "users", Position = 2, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 4, Title = "Ogłoszenia", Link = "announcements", Position = 3, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 5, Title = "Zgłoszenia", Link = "tickets", Position = 4, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 6, Title = "Usprawiedliwienia", Link = "excuses", Position = 5, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 7, Title = "Lekcje", Link = "lessons", Position = 6, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 8, Title = "Plan lekcji", Link = "schedule", Position = 7, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 9, Title = "Oceny", Link = "grades", Position = 8, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 10, Title = "Frekwencja", Link = "attendance", Position = 9, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 11, Title = "Zarządzanie klasami", Link = "classManagement", Position = 10, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 12, Title = "Ustawienia konta", Link = "settings", Position = 11, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 13, Title = "Konfiguracja", Link = "systemConfig", Position = 12, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 14, Title = "Pasek boczny", Link = "layout", Position = 0, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 15, Title = "Login", Link = "login", Position = 0, TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );
        }

        private static void SeedPageContents(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PageContent>().HasData(
                new PageContent { Id = 1, Key = "systemName", Value = "EduPlus", PageId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 2, Key = "pageTitle", Value = "EduPlus - Twój e-dziennik", PageId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 3, Key = "faviconUrl", Value = "logo-64.png", PageId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 4, Key = "title", Value = "Pulpit", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 5, Key = "schoolYear", Value = "Rok szkolny", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 6, Key = "stats.users", Value = "Wszystkich użytkowników", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 7, Key = "stats.students", Value = "Uczniów", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 8, Key = "stats.teachers", Value = "Nauczycieli", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 9, Key = "stats.parents", Value = "Rodziców", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 10, Key = "stats.classes", Value = "Klas", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 11, Key = "quickActions.title", Value = "Szybkie akcje", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 12, Key = "quickActions.manageUsers", Value = "Zarządzaj użytkownikami", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 13, Key = "quickActions.newAnnouncement", Value = "Nowe ogłoszenie", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 14, Key = "quickActions.tickets", Value = "Zgłoszenia", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 15, Key = "tickets.title", Value = "Ostatnie zgłoszenia", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 16, Key = "tickets.viewAll", Value = "Zobacz wszystkie", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 17, Key = "tickets.empty", Value = "Brak zgłoszeń", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 18, Key = "title", Value = "Użytkownicy", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 19, Key = "modal.newUser", Value = "Nowy użytkownik", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 20, Key = "modal.editUser", Value = "Edycja użytkownika", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 21, Key = "modal.manageRelations", Value = "Zarządzanie powiązaniami", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 22, Key = "modal.user", Value = "Użytkownik", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 23, Key = "tabs.users", Value = "Użytkownicy", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 24, Key = "tabs.roles", Value = "Role", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 25, Key = "tabs.relations", Value = "Powiązania", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 26, Key = "columns.user", Value = "Użytkownik", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 27, Key = "columns.email", Value = "Email", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 28, Key = "columns.phone", Value = "Telefon", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 29, Key = "columns.role", Value = "Rola", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 30, Key = "columns.createdAt", Value = "Utworzono", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 31, Key = "columns.updatedAt", Value = "Edytowano", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 32, Key = "columns.modifiedBy", Value = "Edytowane przez", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 33, Key = "columns.actions", Value = "Akcje", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 34, Key = "columns.parent", Value = "Rodzic", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 35, Key = "columns.student", Value = "Uczeń", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 36, Key = "columns.status", Value = "Status", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 37, Key = "columns.roleName", Value = "Nazwa roli", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 38, Key = "columns.level", Value = "Poziom", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 39, Key = "columns.description", Value = "Opis", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 40, Key = "form.role", Value = "Uprawnienia", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 41, Key = "title", Value = "Ogłoszenia", PageId = 4, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 42, Key = "columns.date", Value = "Utworzono", PageId = 4, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 43, Key = "columns.title", Value = "Tytuł", PageId = 4, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 44, Key = "columns.content", Value = "Treść", PageId = 4, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 45, Key = "columns.author", Value = "Autor", PageId = 4, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 46, Key = "columns.modifiedBy", Value = "Edytowane przez", PageId = 4, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 47, Key = "columns.actions", Value = "Akcje", PageId = 4, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 48, Key = "title", Value = "Zgłoszenia", PageId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 124, Key = "columns.email", Value = "Email", PageId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 125, Key = "columns.reason", Value = "Powód", PageId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 126, Key = "columns.status", Value = "Status", PageId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 127, Key = "columns.closedAt", Value = "Data zamknięcia", PageId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 128, Key = "columns.createdAt", Value = "Utworzono", PageId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 130, Key = "columns.modifiedBy", Value = "Zamknięte przez", PageId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 131, Key = "columns.actions", Value = "Akcje", PageId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 49, Key = "title", Value = "Usprawiedliwienia", PageId = 6, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 50, Key = "title", Value = "Lekcje", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 133, Key = "columns.orderNumber", Value = "Nr lekcji", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 134, Key = "columns.class", Value = "Klasa", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 135, Key = "columns.classroom", Value = "Sala", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 136, Key = "columns.subject", Value = "Przedmiot", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 137, Key = "columns.status", Value = "Status", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 138, Key = "columns.createdAt", Value = "Utworzono", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 139, Key = "columns.updatedAt", Value = "Edytowano", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 140, Key = "columns.modifiedBy", Value = "Edytowane przez", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 141, Key = "columns.actions", Value = "Akcje", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 142, Key = "columns.teacher", Value = "Nauczyciel", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 143, Key = "columns.date", Value = "Data", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 144, Key = "columns.time", Value = "Godziny", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 145, Key = "columns.topic", Value = "Temat", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 146, Key = "modal.details", Value = "Lekcja", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 147, Key = "emptyMessage", Value = "Brak danych", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 51, Key = "title", Value = "Plan lekcji", PageId = 8, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 52, Key = "title", Value = "Oceny", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 53, Key = "columns.student", Value = "Uczeń", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 54, Key = "columns.class", Value = "Klasa", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 55, Key = "columns.subject", Value = "Przedmiot", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 56, Key = "columns.grade", Value = "Ocena", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 57, Key = "columns.category", Value = "Kategoria", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 58, Key = "columns.date", Value = "Data", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 59, Key = "title", Value = "Frekwencja", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 60, Key = "columns.student", Value = "Uczeń", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 61, Key = "columns.subject", Value = "Przedmiot", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 62, Key = "columns.status", Value = "Status", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 63, Key = "columns.date", Value = "Data", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 64, Key = "title", Value = "Klasy", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 65, Key = "modal.newClass", Value = "Nowa klasa", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 66, Key = "modal.editClass", Value = "Edycja klasy", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 67, Key = "modal.assignStudents", Value = "Przypisz uczniów", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 68, Key = "modal.assignSubject", Value = "Przypisz przedmiot", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 69, Key = "tabs.students", Value = "Uczniowie", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 70, Key = "tabs.subjects", Value = "Przedmioty", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 71, Key = "form.level", Value = "Poziom", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 72, Key = "form.section", Value = "Oddział", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 73, Key = "form.subject", Value = "Przedmiot", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 74, Key = "form.teacher", Value = "Nauczyciel", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 75, Key = "columns.class", Value = "Klasa", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 76, Key = "columns.studentCount", Value = "Liczba uczniów", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 77, Key = "columns.ordinal", Value = "Lp.", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 78, Key = "columns.student", Value = "Uczeń", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 79, Key = "columns.email", Value = "Email", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 80, Key = "columns.subject", Value = "Przedmiot", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 81, Key = "columns.teacher", Value = "Nauczyciel", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 82, Key = "columns.createdAt", Value = "Utworzono", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 83, Key = "columns.updatedAt", Value = "Edytowano", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 84, Key = "columns.actions", Value = "Akcje", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 85, Key = "title", Value = "Ustawienia konta", PageId = 12, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 86, Key = "section.profile", Value = "Profil", PageId = 12, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 87, Key = "section.password", Value = "Zmiana hasła", PageId = 12, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 132, Key = "title", Value = "Konfiguracja", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null  },
                new PageContent { Id = 88, Key = "modal.new", Value = "Nowy element", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 89, Key = "modal.edit", Value = "Edycja elementu", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 90, Key = "columns.number", Value = "Nr", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 91, Key = "columns.hours", Value = "Godziny", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 92, Key = "columns.symbol", Value = "Symbol", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 93, Key = "columns.name", Value = "Nazwa", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 94, Key = "columns.value", Value = "Wartość", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 95, Key = "columns.weight", Value = "Waga", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 96, Key = "columns.shortCode", Value = "Skrót", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 97, Key = "columns.createdAt", Value = "Utworzono", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 98, Key = "columns.updatedAt", Value = "Edytowano", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 99, Key = "columns.actions", Value = "Akcje", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 100, Key = "systemName", Value = "EduPlus Admin", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 101, Key = "version", Value = "v1.0.0 EduPlus", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 102, Key = "nav.dashboard", Value = "Pulpit", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 103, Key = "nav.section.management", Value = "Zarządzanie", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 104, Key = "nav.users", Value = "Użytkownicy", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 105, Key = "nav.classes", Value = "Klasy", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 106, Key = "nav.announcements", Value = "Ogłoszenia", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 107, Key = "nav.tickets", Value = "Zgłoszenia", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 108, Key = "nav.section.teaching", Value = "Nauczanie", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 109, Key = "nav.schedule", Value = "Plan lekcji", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 110, Key = "nav.lessons", Value = "Lekcje", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 111, Key = "nav.grades", Value = "Oceny", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 112, Key = "nav.attendance", Value = "Frekwencja", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 113, Key = "nav.excuses", Value = "Usprawiedliwienia", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 114, Key = "nav.section.system", Value = "System", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 115, Key = "nav.config", Value = "Konfiguracja", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 116, Key = "nav.cms", Value = "CMS", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 117, Key = "title", Value = "Logowanie", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 118, Key = "logoUrl", Value = "logo-512.png", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 119, Key = "logoAlt", Value = "EduPlus", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 120, Key = "form.email", Value = "Email", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 121, Key = "form.password", Value = "Hasło", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 122, Key = "form.forgotPassword", Value = "Zapomniałeś hasła?", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 123, Key = "footer", Value = "EduPlus v1.0.0", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );
        }

        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Administrator", IsActive = true, Description = "Najwyższy poziom uprawnień", Level = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Role { Id = 2, Name = "Nauczyciel", IsActive = true, Description = "Zarządzanie przydzielonymi klasami", Level = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Role { Id = 3, Name = "Rodzic", IsActive = true, Description = "Dostęp do dziennika dziecka", Level = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Role { Id = 4, Name = "Uczeń", IsActive = true, Description = "Dostęp do własnego dziennika", Level = 4, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );
        }

        private static void SeedSchoolYears(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchoolYear>().HasData(
                new SchoolYear { Id = 1, Name = "2025/2026", StartDate = new DateOnly(2025, 9, 1), EndDate = new DateOnly(2026, 6, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new SchoolYear { Id = 2, Name = "2026/2027", StartDate = new DateOnly(2026, 9, 1), EndDate = new DateOnly(2027, 6, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );
        }

        private static void SeedAttendanceTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendanceType>().HasData(
                new AttendanceType { Id = 1, Name = "Obecność", ShortCode = "OB", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new AttendanceType { Id = 2, Name = "Nieobecność", ShortCode = "NB", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new AttendanceType { Id = 3, Name = "Spóźnienie", ShortCode = "SP", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new AttendanceType { Id = 4, Name = "Usprawiedliwione", ShortCode = "U", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new AttendanceType { Id = 5, Name = "Zwolnienie", ShortCode = "ZW", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );
        }

        private static void SeedGradeTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GradeType>().HasData(
                new GradeType { Id = 1, Numeric = "1", Name = "Niedostateczny", Value = 1.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeType { Id = 2, Numeric = "2", Name = "Dopuszczający", Value = 2.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeType { Id = 3, Numeric = "3", Name = "Dostateczny", Value = 3.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeType { Id = 4, Numeric = "4", Name = "Dobry", Value = 4.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeType { Id = 5, Numeric = "5", Name = "Bardzo dobry", Value = 5.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeType { Id = 6, Numeric = "6", Name = "Celujący", Value = 6.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );
        }

        private static void SeedGradeCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GradeCategory>().HasData(
                new GradeCategory { Id = 1, Name = "Sprawdzian", Weight = 3, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeCategory { Id = 2, Name = "Kartkówka", Weight = 2, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeCategory { Id = 3, Name = "Odpowiedź ustna", Weight = 1, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeCategory { Id = 4, Name = "Aktywność", Weight = 1, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeCategory { Id = 5, Name = "Zadanie domowe", Weight = 1, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );
        }

        private static void SeedLessonStatuses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LessonStatus>().HasData(
                new LessonStatus { Id = 1, Name = "Zaplanowana", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new LessonStatus { Id = 2, Name = "Zrealizowana", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new LessonStatus { Id = 3, Name = "Odwołana", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );
        }

        private static void SeedLessonHours(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LessonHour>().HasData(
                new LessonHour { Id = 1, OrderNumber = 1, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(8, 45), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new LessonHour { Id = 2, OrderNumber = 2, StartTime = new TimeOnly(8, 55), EndTime = new TimeOnly(9, 40), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new LessonHour { Id = 3, OrderNumber = 3, StartTime = new TimeOnly(9, 50), EndTime = new TimeOnly(10, 35), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new LessonHour { Id = 4, OrderNumber = 4, StartTime = new TimeOnly(10, 45), EndTime = new TimeOnly(11, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new LessonHour { Id = 5, OrderNumber = 5, StartTime = new TimeOnly(11, 45), EndTime = new TimeOnly(12, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new LessonHour { Id = 6, OrderNumber = 6, StartTime = new TimeOnly(12, 50), EndTime = new TimeOnly(13, 35), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new LessonHour { Id = 7, OrderNumber = 7, StartTime = new TimeOnly(13, 45), EndTime = new TimeOnly(14, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new LessonHour { Id = 8, OrderNumber = 8, StartTime = new TimeOnly(14, 40), EndTime = new TimeOnly(15, 25), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new LessonHour { Id = 9, OrderNumber = 9, StartTime = new TimeOnly(15, 30), EndTime = new TimeOnly(16, 15), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );
        }

        private static void SeedSemesters(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Semester>().HasData(
                new Semester { Id = 1, SchoolYearId = 1, Name = "Semestr 1", StartDate = new DateOnly(2025, 09, 01), EndDate = new DateOnly(2026, 01, 31), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Semester { Id = 2, SchoolYearId = 1, Name = "Semestr 2", StartDate = new DateOnly(2026, 02, 01), EndDate = new DateOnly(2026, 06, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Semester { Id = 3, SchoolYearId = 2, Name = "Semestr 1", StartDate = new DateOnly(2026, 09, 01), EndDate = new DateOnly(2027, 01, 31), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Semester { Id = 4, SchoolYearId = 2, Name = "Semestr 2", StartDate = new DateOnly(2027, 02, 01), EndDate = new DateOnly(2027, 06, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );
        }

        private static void SeedTicketReasons(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TicketReason>().HasData(
                new TicketReason { Id = 1, Name = "Problem z logowaniem", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new TicketReason { Id = 2, Name = "Zmiana danych osobowych", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new TicketReason { Id = 3, Name = "Błąd w systemie", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new TicketReason { Id = 4, Name = "Inne", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );
        }
    }
}

