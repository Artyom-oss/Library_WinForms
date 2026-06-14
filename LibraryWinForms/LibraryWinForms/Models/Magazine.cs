namespace LibraryWinForms.Models
{
    public class Magazine : LibraryItem
    {
        public int Issue { get; set; }

        public override string GetInfo()
        {
            return $"ID:{Id} | Журнал | {Title} | {Year} | Выпуск:{Issue}";
        }
    }
}