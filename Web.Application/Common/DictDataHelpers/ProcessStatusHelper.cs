namespace Web.Application.Common.DictDataHelpers
{
    public static class ProcessStatusHelper
    {
        public static string GetCssClass(byte processStatusId)
        {
            if (processStatusId == 1)
            {
                return "badge bg-dark ";
            }
            else if (processStatusId == 2)
            {
                return "badge badge-subtle-info ";
            }
            else if (processStatusId == 3)
            {
                return "badge badge-subtle-success";
            }
            else if (processStatusId == 4)
            {
                return "badge badge-subtle-danger";
            }

            return "";
        }
    }
}
