using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace Letu.ChatPdf;

public class PdfParagraphRepository
{
    /// <summary>
    /// 插入embedding向量
    /// </summary>
    /// <param name="content"></param>
    /// <param name="vector"></param>
    /// <returns></returns>
    public async Task<PdfParagraph> InsertAsync(PdfParagraph pdfParagraph)
    {
        using var db = new ChatPdfDbContext();
        db.PdfParagraphs.Add(pdfParagraph);
        await db.SaveChangesAsync();

        return pdfParagraph;
    }

    /// <summary>
    /// 查询embedding向量
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="maxResult"></param>
    /// <returns></returns>
    public async Task<List<ParagraphFindResult>> FindAsync(Vector vector, int maxResult = 5)
    {
        using var db = new ChatPdfDbContext();
        return await db.PdfParagraphs
            .Select(x => new ParagraphFindResult { Id =x.Id, Content=x.Content , Distance = x.Vector!.L2Distance(vector) })
            .OrderBy(x => x.Distance)
            .Take(maxResult)
            .ToListAsync();
    }

    /// <summary>
    /// 更新embedding向量
    /// </summary>
    /// <param name="id"></param>
    /// <param name="content"></param>
    /// <param name="vector"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<PdfParagraph> UpdateAsync(int id, string content, Vector vector)
    {
        using var db = new ChatPdfDbContext();
        var pdfParagraph = await db.PdfParagraphs.FindAsync(id);
        if (pdfParagraph != null)
        {
            pdfParagraph.Content = content;
            pdfParagraph.Vector = vector;
            await db.SaveChangesAsync();
            return pdfParagraph;
        }

        throw new Exception($"Paragraph id:{id} not found.");
    }

    /// <summary>
    /// 删除embedding向量
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task Delete(int id)
    {
        using var db = new ChatPdfDbContext();
        var pdfParagraph = await db.PdfParagraphs.FindAsync(id);
        if (pdfParagraph != null)
        {
            db.PdfParagraphs.Remove(pdfParagraph);
            await db.SaveChangesAsync();
        }
    }

}
