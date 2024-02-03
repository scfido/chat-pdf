using Pgvector;
using System.CommandLine;

namespace Letu.ChatPdf.Cli
{
    public class QueryCommand
    {
        public static Command Create()
        {
            var command = new Command("query", "查询匹配的文本");
            var queryOption = new Option<string>("--query", "查询的文本");
            var limitOption = new Option<int>("--limit", "最大返回数据条数");
            limitOption.SetDefaultValue(5);
            command.AddOption(queryOption);
            command.AddOption(limitOption);
            command.SetHandler(QueryAsync, queryOption, limitOption);
            return command;
        }

        public static async Task QueryAsync(string query, int limit)
        {
            IEmbbedingService embbedingService = new HunyuanEmbbedingService();
            var repository = new PdfParagraphRepository();
            var vector = await embbedingService.GetVectorAsync(query);
            if (vector == null)
            {
                Console.WriteLine($"获取{query}的向量失败");
                return;
            }
            var ps = await repository.FindAsync(new Vector(vector), limit);

            foreach (var p in ps)
            {
                Console.WriteLine($"{p.Distance} {p.Content}");
            }
        }
    }
}
