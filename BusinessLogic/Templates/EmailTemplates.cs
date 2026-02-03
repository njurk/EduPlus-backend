namespace BusinessLogic.Templates
{
    public static class EmailTemplates
    {
        private const string Footer = "<br/><p>Pozdrawiamy,<br/>Zespół EduPlus</p>";

        public static string PasswordReset(string resetLink) => $@"
            <p>Otrzymaliśmy prośbę o reset hasła do twojego konta w serwisie EduPlus. Kliknij poniższy link, aby zresetować hasło:</p>
            <p><a href=""{resetLink}"">{resetLink}</a></p>
            <p>Link jest ważny przez 1 godzinę</p>
            <p>Jeśli nie prosiłeś o reset hasła, zignoruj tę wiadomość.</p>
            {Footer}";

        public static string TicketCreated(int ticketNumber, string reason, string content) => $@"
            <h2>Twoje zgłoszenie zostało przyjęte</h2>
            <p><strong>Numer zgłoszenia:</strong> #{ticketNumber}</p>
            <p><strong>Powód:</strong> {reason}</p>
            <p><strong>Treść zgłoszenia:</strong></p>
            <div style=""background-color: #f5f5f5; padding: 12px; border-radius: 4px; margin: 8px 0;"">{content}</div>
            <p>Dziękujemy za przesłanie zgłoszenia. Nasz zespół zajmie się nim najszybciej jak to możliwe.</p>
            {Footer}";

        public static string TicketClosed(int ticketNumber, string reason, string content, string adminResponse, string resolvedBy) => $@"
            <h2>Twoje zgłoszenie zostało rozpatrzone</h2>
            <p><strong>Numer zgłoszenia:</strong> #{ticketNumber}</p>
            <p><strong>Powód:</strong> {reason}</p>
            <p><strong>Treść zgłoszenia:</strong></p>
            <div style=""background-color: #f5f5f5; padding: 12px; border-radius: 4px; margin: 8px 0;"">{content}</div>
            <p><strong>Rozpatrzone przez:</strong> {resolvedBy}</p>
            <p><strong>Odpowiedź:</strong></p>
            <div style=""background-color: #e8f5e9; padding: 12px; border-radius: 4px; margin: 8px 0;"">{adminResponse}</div>
            {Footer}";

        public static string NewGrade(string studentName, string subjectName, string gradeValue, string teacherName, string issueDate) => $@"
            <h2>Nowa ocena</h2>
            <p><strong>Uczeń:</strong> {studentName}</p>
            <p><strong>Przedmiot:</strong> {subjectName}</p>
            <p><strong>Ocena:</strong> {gradeValue}</p>
            <p><strong>Wystawił:</strong> {teacherName}</p>
            <p><strong>Data:</strong> {issueDate}</p>
            {Footer}";

        public static string NegativeAttendance(string studentName, string subjectName, string attendanceType, string teacherName, string date) => $@"
            <h2>Nowy wpis frekwencji</h2>
            <p><strong>Uczeń:</strong> {studentName}</p>
            <p><strong>Przedmiot:</strong> {subjectName}</p>
            <p><strong>Typ:</strong> {attendanceType}</p>
            <p><strong>Nauczyciel:</strong> {teacherName}</p>
            <p><strong>Data:</strong> {date}</p>
            {Footer}";
    }
}
