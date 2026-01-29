using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentGradeApi.Migrations
{
    /// <inheritdoc />
    public partial class Add1stYearCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var year1 = new[] { 
                "الثقافه البيئيه", "ثقافة الجودة", "رياضيات 2", "رياضيات تطبيقية", 
                "فيزياء 2", "لغه انجليزية علميه", "مدخل الحاسب الآلى" 
            };
            foreach (var name in year1)
                migrationBuilder.Sql($"UPDATE \"courses\" SET \"year_num\" = 1 WHERE \"course_name\" = '{name}';");

            // ثالثاً: تحديث السنة الثانية للمواد المحددة فقط
            var year3 = new[] { "مفاهيم لغات البرمجه" };
            foreach (var name in year3)
                migrationBuilder.Sql($"UPDATE \"courses\" SET \"year_num\" = 3 WHERE \"course_name\" = '{name}';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var allCourses = new[] { 
                "الثقافه البيئيه", "ثقافة الجودة", "رياضيات 2", "رياضيات تطبيقية", 
                "فيزياء 2", "لغه انجليزية علميه", "مدخل الحاسب الآلى", "مفاهيم لغات البرمجه" 
            };

            foreach (var name in allCourses)
            {
                migrationBuilder.Sql($"UPDATE \"Course\" SET \"year_num\" = 0 WHERE \"course_name\" = '{name}';");
            }
        }
    }
}
