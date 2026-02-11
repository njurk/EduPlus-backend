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
            modelBuilder.Entity<Target>().HasData(
                new Target { Id = 1, Label = "WebAdmin", Title = "Administrator", CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Target { Id = 2, Label = "WebTeacher", Title = "Nauczyciel", CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Target { Id = 3, Label = "Mobile", Title = "Rodzic/Uczeń", CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Target { Id = 5, Label = "All", Title = "Wspólne", CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );

            modelBuilder.Entity<Page>().HasData(
                new Page { Id = 1, Title = "System", Link = "system", TargetId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 16, Title = "Reset hasła", Link = "resetPassword", TargetId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 17, Title = "Zgłoś problem", Link = "submitTicket", TargetId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new Page { Id = 14, Title = "Nawigacja", Link = "layout", TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 18, Title = "Logowanie", Link = "adminLogin", TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 2, Title = "Dashboard", Link = "dashboard", TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 3, Title = "Użytkownicy", Link = "users", TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 11, Title = "Zarządzanie klasami", Link = "classManagement", TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 19, Title = "Przedmioty", Link = "subjects", TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 4, Title = "Ogłoszenia szkolne", Link = "announcements", TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 5, Title = "Zgłoszenia", Link = "tickets", TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 8, Title = "Plany lekcji", Link = "schedule", TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 7, Title = "Lekcje", Link = "lessons", TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 9, Title = "Oceny", Link = "grades", TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 10, Title = "Frekwencja", Link = "attendance", TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 6, Title = "Usprawiedliwienia", Link = "excuses", TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 13, Title = "Konfiguracja", Link = "systemConfig", TargetId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new Page { Id = 30, Title = "Nawigacja", Link = "teacherLayout", TargetId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 15, Title = "Logowanie", Link = "teacherLogin", TargetId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new Page { Id = 26, Title = "Logowanie", Link = "mobileLogin", TargetId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 20, Title = "Pulpit", Link = "mobileDashboard", TargetId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 21, Title = "Oceny", Link = "mobileGrades", TargetId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 22, Title = "Frekwencja", Link = "mobileAttendance", TargetId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 23, Title = "Plan lekcji", Link = "mobileSchedule", TargetId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 24, Title = "Ogłoszenia", Link = "mobileAnnouncements", TargetId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 25, Title = "Usprawiedliwienia", Link = "mobileExcuses", TargetId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 27, Title = "Ustawienia", Link = "mobileSettings", TargetId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 28, Title = "Pojedyncze ogłoszenie", Link = "mobileAnnouncementDetail", TargetId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 29, Title = "Formularz usprawiedliwienia", Link = "mobileExcuseForm", TargetId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Page { Id = 31, Title = "Zgłoszenie problemu", Link = "mobileSubmitTicket", TargetId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );

            modelBuilder.Entity<PageContent>().HasData(
                new PageContent { Id = 1, Key = "systemName", Value = "EduPlus", PageId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 2, Key = "pageTitle", Value = "EduPlus - Twój e-dziennik", PageId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 3, Key = "faviconUrl", Value = "logo-64.png", PageId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 120, Key = "logoUrl", Value = "logo-512.png", PageId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 37, Key = "version", Value = "v1.0.0", PageId = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 4, Key = "title", Value = "Pulpit", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 11, Key = "quickActions.title", Value = "Szybkie akcje", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 15, Key = "tickets.title", Value = "Ostatnie zgłoszenia", PageId = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 18, Key = "title", Value = "Użytkownicy", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 77, Key = "tabs.users", Value = "Użytkownicy", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 78, Key = "tabs.roles", Value = "Role", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 79, Key = "tabs.relations", Value = "Powiązania", PageId = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 19, Key = "title", Value = "Ogłoszenia", PageId = 4, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 20, Key = "title", Value = "Zgłoszenia", PageId = 5, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 21, Key = "title", Value = "Usprawiedliwienia", PageId = 6, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 22, Key = "title", Value = "Lekcje", PageId = 7, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 23, Key = "title", Value = "Plan lekcji", PageId = 8, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 24, Key = "days.monday", Value = "Poniedziałek", PageId = 8, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 25, Key = "days.tuesday", Value = "Wtorek", PageId = 8, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 26, Key = "days.wednesday", Value = "Środa", PageId = 8, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 27, Key = "days.thursday", Value = "Czwartek", PageId = 8, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 28, Key = "days.friday", Value = "Piątek", PageId = 8, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 29, Key = "title", Value = "Oceny", PageId = 9, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 30, Key = "title", Value = "Frekwencja", PageId = 10, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 31, Key = "title", Value = "Klasy", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 90, Key = "tabs.students", Value = "Uczniowie", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 91, Key = "tabs.subjects", Value = "Przydział przedmiotów", PageId = 11, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 35, Key = "title", Value = "Konfiguracja", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 80, Key = "tabs.schoolYears", Value = "Rok szkolny", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 81, Key = "tabs.classrooms", Value = "Sale", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 82, Key = "tabs.subjects", Value = "Przedmioty", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 83, Key = "tabs.lessonHours", Value = "Godziny lekcyjne", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 84, Key = "tabs.lessonStatuses", Value = "Statusy lekcji", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 85, Key = "tabs.gradeTypes", Value = "Oceny", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 86, Key = "tabs.gradeCategories", Value = "Kategorie ocen", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 132, Key = "tabs.gradingScale", Value = "Skala oceniania", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 87, Key = "tabs.attendance", Value = "Frekwencja", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 88, Key = "tabs.ticketReasons", Value = "Powody zgłoszeń", PageId = 13, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 36, Key = "systemName", Value = "EduPlus Admin", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 38, Key = "nav.dashboard", Value = "Pulpit", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 39, Key = "nav.section.management", Value = "Zarządzanie", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 40, Key = "nav.users", Value = "Użytkownicy", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 41, Key = "nav.classes", Value = "Klasy", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 92, Key = "nav.subjects", Value = "Przydział przedmiotów", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 42, Key = "nav.announcements", Value = "Ogłoszenia", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 43, Key = "nav.tickets", Value = "Zgłoszenia", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 44, Key = "nav.section.teaching", Value = "Nauczanie", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 45, Key = "nav.schedule", Value = "Plan lekcji", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 46, Key = "nav.lessons", Value = "Lekcje", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 47, Key = "nav.grades", Value = "Oceny", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 48, Key = "nav.attendance", Value = "Frekwencja", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 49, Key = "nav.excuses", Value = "Usprawiedliwienia", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 50, Key = "nav.section.system", Value = "System", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 51, Key = "nav.config", Value = "Konfiguracja", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 52, Key = "nav.cms", Value = "CMS", PageId = 14, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 53, Key = "title", Value = "EduPlus", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 55, Key = "logoAlt", Value = "EduPlus", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 56, Key = "form.forgotPassword", Value = "Zapomniałeś hasła?", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 58, Key = "helpLink", Value = "Masz problem? Kliknij tutaj", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 59, Key = "adminLink", Value = "Jestem administratorem", PageId = 15, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 60, Key = "title.request", Value = "Resetowanie hasła", PageId = 16, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 61, Key = "title.sent", Value = "Sprawdź swoją skrzynkę", PageId = 16, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 62, Key = "title.reset", Value = "Ustaw nowe hasło", PageId = 16, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 63, Key = "message.sent", Value = "Jeśli adres {email} istnieje w naszej bazie, za chwilę otrzymasz wiadomość z linkiem do resetowania hasła.", PageId = 16, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 64, Key = "message.linkExpirationTime", Value = "Link wygasa po 1 godzinie", PageId = 16, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 108, Key = "subtitle", Value = "Podaj adres email którego używasz w systemie EduPlus", PageId = 16, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 65, Key = "title", Value = "Zgłoś problem", PageId = 17, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 66, Key = "subtitle", Value = "Masz problem z logowaniem lub chcesz zgłosić inny problem? Wypełnij formularz poniżej.", PageId = 17, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 67, Key = "form.description", Value = "Opis problemu", PageId = 17, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 68, Key = "form.descriptionPlaceholder", Value = "Opisz szczegółowo problem który napotkałeś", PageId = 17, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 69, Key = "success.message", Value = "Twoje zgłoszenie zostało przyjęte. Odpowiedź otrzymasz na podany adres email.", PageId = 17, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 109, Key = "title", Value = "Pomoc techniczna", PageId = 31, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 110, Key = "subtitle", Value = "Wyślij do nas wiadomość", PageId = 31, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 111, Key = "success.title", Value = "Zgłoszenie wysłane", PageId = 31, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 112, Key = "success.message", Value = "Dziękujemy za kontakt. Odpowiemy najszybciej jak to możliwe.", PageId = 31, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 70, Key = "title", Value = "EduPlus Admin", PageId = 18, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 72, Key = "logoAlt", Value = "EduPlus", PageId = 18, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 73, Key = "form.forgotPassword", Value = "Zapomniałeś hasła?", PageId = 18, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 75, Key = "helpLink", Value = "Masz problem? Kliknij tutaj", PageId = 18, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 76, Key = "teacherLink", Value = "Jestem nauczycielem", PageId = 18, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 89, Key = "title", Value = "Przedmioty", PageId = 19, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 93, Key = "title", Value = "Pulpit", PageId = 20, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 99, Key = "greeting", Value = "Witaj, {name}!", PageId = 20, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 100, Key = "studentLabel", Value = "Uczeń:", PageId = 20, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 101, Key = "lessonsToday", Value = "Lekcje dzisiaj ({day})", PageId = 20, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 102, Key = "recentGrades", Value = "Ostatnie oceny", PageId = 20, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 103, Key = "recentAttendance", Value = "Ostatnia frekwencja", PageId = 20, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 104, Key = "recentAnnouncements", Value = "Ostatnie ogłoszenia", PageId = 20, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 94, Key = "title", Value = "Oceny", PageId = 21, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 95, Key = "title", Value = "Frekwencja", PageId = 22, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 116, Key = "tabs.week", Value = "Tydzień", PageId = 22, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 117, Key = "tabs.stats", Value = "Statystyki", PageId = 22, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 96, Key = "title", Value = "Plan lekcji", PageId = 23, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 97, Key = "title", Value = "Ogłoszenia", PageId = 24, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 98, Key = "title", Value = "Usprawiedliwienia", PageId = 25, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 118, Key = "section.unexcused", Value = "Do usprawiedliwienia", PageId = 25, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 119, Key = "section.excused", Value = "Wysłane", PageId = 25, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 105, Key = "subtitle", Value = "Twój e-dziennik", PageId = 26, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 106, Key = "link.forgotPassword", Value = "Zapomniałeś hasła?", PageId = 26, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 107, Key = "link.submitTicket", Value = "Masz problem? Kliknij tutaj", PageId = 26, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 113, Key = "title", Value = "Ustawienia", PageId = 27, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 114, Key = "title", Value = "Ogłoszenie", PageId = 28, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 115, Key = "title", Value = "Usprawiedliwienie", PageId = 29, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },

                new PageContent { Id = 121, Key = "nav.dashboard", Value = "Pulpit", PageId = 30, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 122, Key = "nav.section.teaching", Value = "Nauczanie", PageId = 30, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 123, Key = "nav.schedule", Value = "Plan lekcji", PageId = 30, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 124, Key = "nav.registry", Value = "Dziennik", PageId = 30, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 125, Key = "nav.excuses", Value = "Usprawiedliwienia", PageId = 30, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 126, Key = "nav.announcements", Value = "Ogłoszenia", PageId = 30, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 127, Key = "title.dashboard", Value = "Pulpit nauczyciela", PageId = 30, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 128, Key = "title.schedule", Value = "Plan lekcji", PageId = 30, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 129, Key = "title.registry", Value = "Dziennik", PageId = 30, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 130, Key = "title.announcements", Value = "Ogłoszenia", PageId = 30, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new PageContent { Id = 131, Key = "title.excuses", Value = "Usprawiedliwienia", PageId = 30, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Administrator", IsActive = true, Description = "Najwyższy poziom uprawnień", Level = 1, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Role { Id = 2, Name = "Nauczyciel", IsActive = true, Description = "Zarządzanie przydzielonymi klasami", Level = 2, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Role { Id = 3, Name = "Rodzic", IsActive = true, Description = "Dostęp do dziennika dziecka", Level = 3, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Role { Id = 4, Name = "Uczeń", IsActive = true, Description = "Dostęp do własnego dziennika", Level = 4, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );

            modelBuilder.Entity<SchoolYear>().HasData(
                new SchoolYear { Id = 1, Name = "2025/2026", StartDate = new DateOnly(2025, 9, 1), EndDate = new DateOnly(2026, 6, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new SchoolYear { Id = 2, Name = "2026/2027", StartDate = new DateOnly(2026, 9, 1), EndDate = new DateOnly(2027, 6, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );

            modelBuilder.Entity<AttendanceType>().HasData(
                new AttendanceType { Id = 1, Name = "Obecność", Slug = "present", ShortCode = "OB", ColorHex = "#22c55e", IsNegative = false, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new AttendanceType { Id = 2, Name = "Nieobecność", Slug = "absent", ShortCode = "NB", ColorHex = "#ef4444", IsNegative = true, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new AttendanceType { Id = 3, Name = "Spóźnienie", Slug = "late", ShortCode = "SP", ColorHex = "#eab308", IsNegative = true, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new AttendanceType { Id = 4, Name = "Usprawiedliwione", Slug = "excused", ShortCode = "U", ColorHex = "#8b5cf6", IsNegative = true, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new AttendanceType { Id = 5, Name = "Zwolnienie", Slug = "released", ShortCode = "ZW", ColorHex = "#3b82f6", IsNegative = true, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );

            modelBuilder.Entity<GradeType>().HasData(
                new GradeType { Id = 1, Numeric = "1", Name = "Niedostateczny", Value = 1.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeType { Id = 2, Numeric = "2", Name = "Dopuszczający", Value = 2.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeType { Id = 3, Numeric = "3", Name = "Dostateczny", Value = 3.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeType { Id = 4, Numeric = "4", Name = "Dobry", Value = 4.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeType { Id = 5, Numeric = "5", Name = "Bardzo dobry", Value = 5.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeType { Id = 6, Numeric = "6", Name = "Celujący", Value = 6.0m, IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );

            modelBuilder.Entity<GradeCategory>().HasData(
                new GradeCategory { Id = 1, Name = "Sprawdzian", Weight = 3, ColorHex = "#ef4444", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeCategory { Id = 2, Name = "Kartkówka", Weight = 2, ColorHex = "#22c55e", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeCategory { Id = 3, Name = "Odpowiedź ustna", Weight = 1, ColorHex = "#f97316", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeCategory { Id = 4, Name = "Aktywność", Weight = 1, ColorHex = "#3b82f6", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeCategory { Id = 5, Name = "Zadanie domowe", Weight = 1, ColorHex = "#eab308", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeCategory { Id = 6, Name = "Ocena śródroczna", Weight = 0, ColorHex = "#646464ff", Slug = "midyear", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new GradeCategory { Id = 7, Name = "Ocena roczna", Weight = 0, ColorHex = "#646464ff", Slug = "final", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );

            modelBuilder.Entity<GradingScale>().HasData(
                new GradingScale { Id = 1, GradeTypeId = 1, MinAverage = 0.00m, MaxAverage = 1.59m, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new GradingScale { Id = 2, GradeTypeId = 2, MinAverage = 1.60m, MaxAverage = 2.59m, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new GradingScale { Id = 3, GradeTypeId = 3, MinAverage = 2.60m, MaxAverage = 3.59m, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new GradingScale { Id = 4, GradeTypeId = 4, MinAverage = 3.60m, MaxAverage = 4.59m, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new GradingScale { Id = 5, GradeTypeId = 5, MinAverage = 4.60m, MaxAverage = 5.29m, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime },
                new GradingScale { Id = 6, GradeTypeId = 6, MinAverage = 5.30m, MaxAverage = 6.00m, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime }
            );

            modelBuilder.Entity<LessonStatus>().HasData(
                new LessonStatus { Id = 1, Name = "Zrealizowana", Slug = "completed", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new LessonStatus { Id = 2, Name = "Odwołana", Slug = "cancelled", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new LessonStatus { Id = 3, Name = "Zastępstwo", Slug = "substitute", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );

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

            modelBuilder.Entity<Semester>().HasData(
                new Semester { Id = 1, SchoolYearId = 1, Name = "Semestr 1", StartDate = new DateOnly(2025, 09, 01), EndDate = new DateOnly(2026, 02, 28), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Semester { Id = 2, SchoolYearId = 1, Name = "Semestr 2", StartDate = new DateOnly(2026, 03, 01), EndDate = new DateOnly(2026, 06, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Semester { Id = 3, SchoolYearId = 2, Name = "Semestr 1", StartDate = new DateOnly(2026, 09, 01), EndDate = new DateOnly(2027, 02, 28), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new Semester { Id = 4, SchoolYearId = 2, Name = "Semestr 2", StartDate = new DateOnly(2027, 03, 01), EndDate = new DateOnly(2027, 06, 30), IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );

            modelBuilder.Entity<TicketReason>().HasData(
                new TicketReason { Id = 1, Name = "Problem z logowaniem", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new TicketReason { Id = 2, Name = "Błąd w systemie", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null },
                new TicketReason { Id = 3, Name = "Inne", IsActive = true, CreatedAt = InitialDateTime, UpdatedAt = InitialDateTime, ModifiedByUserId = null }
            );
        }
    }
}
