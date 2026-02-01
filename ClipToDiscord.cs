using System;
using System.Net.Http;
using System.Text;
using System.Threading;

public class CPHInline
{
    public bool Execute()
    {
        // 1. RÉCUPÉRATION DU CLIP (Déjà validé par ton log, mais on garde la sécurité)
        string clipUrl = args.ContainsKey("createClipUrl") ? args["createClipUrl"].ToString() : "";
        
        if (string.IsNullOrEmpty(clipUrl)) return false;

        // 2. RÉCUPÉRATION DES INFOS 
        string gameName = args.ContainsKey("game") ? args["game"].ToString() : "un jeu génial";
        string userName = args.ContainsKey("user") ? args["user"].ToString() : "Geek_Des_Bois";
        string webhookUrl = CPH.GetGlobalVar<string>("discordWebhook", true);

        // 3. ENVOI DISCORD
        if (!string.IsNullOrEmpty(webhookUrl))
        {
            try {
                using (var client = new HttpClient())
                {
                    // Message propre pour Discord
                    string discordMsg = "🎬 **Nouveau Clip par " + userName + " !**\\n🎮 **Jeu :** " + gameName + "\\n🔗 " + clipUrl;
                    string jsonBody = "{\"content\": \"" + discordMsg + "\"}";
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    var response = client.PostAsync(webhookUrl, content).Result;
                }
            } catch (Exception ex) { CPH.LogWarn("Erreur Discord : " + ex.Message); }
        }

        // 4. MESSAGE TWITCH (Le lien direct + ton invitation)
        CPH.SendMessage($"✅ @{userName} Ton clip sur {gameName} est prêt ! {clipUrl} | Check le Discord : https://discord.gg/zgYj7uKRym");

        return true;
    }
}
