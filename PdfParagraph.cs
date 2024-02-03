using Pgvector;
using System.ComponentModel.DataAnnotations.Schema;

namespace Letu.ChatPdf;

public class PdfParagraph
{
    public int Id { get; set; }

    public string Content { get; set; }

    public Vector? Vector { get; set; }
}
