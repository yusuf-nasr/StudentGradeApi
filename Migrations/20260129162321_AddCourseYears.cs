using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentGradeApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseYears : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "year_num",
                table: "courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_course_name",
                table: "enrollments",
                column: "course_name");

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_student_id",
                table: "enrollments",
                column: "student_id");

            // Add Year Num for each course
            // Year 1
            migrationBuilder.Sql("UPDATE \"courses\" SET \"year_num\" = 1 WHERE \"course_name\" = 'علوم طبيعية';");

            // Year 2
            var year2Courses = new[] {
                "انظمه الحاسب", "برمجه حاسب", "جبر مجرد", "تحليل رياضى (1)", 
                "مهارات الترجمه", "رياضيات متقطعه", "اعداد الملفات", "تحليل رياضى 2", 
                "جبر خطى وهندسه المجسمات", "التصميم المنطقى", "انشاء البيانات والخوارزميات", 
                "انظمه قواعد البيانات", "البرمجه الموجهه"
            };
            foreach (var name in year2Courses)
            {
                migrationBuilder.Sql($"UPDATE \"courses\" SET \"year_num\" = 2 WHERE \"course_name\" = '{name}';");
            }

            // Year 3
            var year3Courses = new[] {
                "تحليل عددى 1", "المنطق الرياضى والجبر البولى", "نظام الحاسب وبرمجه لغات التجميع", 
                "تصميم وتحليل الخوارزمات", "جبر مجرد وتوبولوجى", "نظريه التوافقيات والبيانات", 
                "نظريه الاليات", "انظمه التشغيل", "بنية الحاسب", "محاكاه النظم", 
                "التحكم الأمثل (1)", "تحليل وتصميم النظم"
            };
            foreach (var name in year3Courses)
            {
                migrationBuilder.Sql($"UPDATE \"courses\" SET \"year_num\" = 3 WHERE \"course_name\" = '{name}';");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_enrollments_course_name",
                table: "enrollments");

            migrationBuilder.DropIndex(
                name: "IX_enrollments_student_id",
                table: "enrollments");

            migrationBuilder.DropColumn(
                name: "year_num",
                table: "courses");

            migrationBuilder.Sql("UPDATE \"courses\" SET \"year_num\" = 0;");
        }
    }
}
