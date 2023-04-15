using System;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

public class AsciiTableCommandModule : BaseCommandModule {

    private void generateTable(ref string result, byte min=32, byte max=255){
        for (byte i = min; i <= max; i++)
        {
            var c = (char)i;
            result += $"'{i.ToString()}' = '{c.ToString()}'";
            if (i!=max) { result += " | "; } else {Console.WriteLine();}
        }
    }

    [Command("asciitable")]
    public async Task AsciiTableCommand(CommandContext ctx, byte min=32, byte max=255 ) {
        string response = "";
        generateTable(ref response, min,max);
        await ctx.RespondAsync(response);
    }
}