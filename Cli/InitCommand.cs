using Pgvector;
using System.CommandLine;

namespace Letu.ChatPdf.Cli
{
    public class InitCommand
    {
        public static Command Create()
        {
            var command = new Command("init", "初始化数据库")
            {
                new Option<string>("--connectionString", "数据库连接字符串"),
            };
            command.SetHandler(InitAsync);
            return command;
        }

        public static async Task InitAsync()
        {
            IEmbbedingService embbedingService = new HunyuanEmbbedingService();
            var repository = new PdfParagraphRepository();
            string[] contents = [
                "法国巴黎一火车站发生袭击事件 造成3人受伤 持刀行凶者被逮捕",
                "消息人士称美军袭击地区无伊朗伊斯兰革命卫队基地",
                "美军空袭伊叙境内目标或招致更多报复性袭击",
                "土耳其总统和瑞典首相通电话 讨论瑞典加入北约进程",
                "全球发展和南南合作基金支持的首个蒙古国项目启动",
                "上海浦东机场网约车禁令下“空港出行”一枝独秀？市交通委介入处理",
                "多趟高铁因大雪停车：有人花两千元改签机票，有人耽搁10多个小时",
                "县委原书记主动投案4个多月后，原县长主动向组织交代问题",
                "我国无偿献血人数大幅下降？无偿献血用于血液制品出口？官方辟谣"
                ];

            foreach (var content in contents)
            {
                var vector = await embbedingService.GetVectorAsync(content);
                if (vector == null)
                {
                    Console.WriteLine($"获取{content}的向量失败");
                    continue;
                }
                else
                {
                    await repository.InsertAsync(new PdfParagraph
                    {
                        Content = content,
                        Vector = new Vector(vector)
                    });
                }
            }
        }
    }
}
