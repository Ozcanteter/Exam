using System.Threading.Tasks;

namespace Exam.Data;

public interface IExamDbSchemaMigrator
{
    Task MigrateAsync();
}
