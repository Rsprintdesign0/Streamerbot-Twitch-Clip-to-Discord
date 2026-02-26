using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class CPHInline
{
    public bool Execute()
    {
        // 1. RÉCUPÉRATION DU WEBHOOK
        // On récupère l'URL stockée en variable globale persistante
        string webhookUrl = CPH.GetGlobalVar<string>("discordWebhook", true);
        
        if (string.IsNullOrEmpty(webhookUrl))
        {
            CPH.LogError("❌ [CLIP] Erreur : La variable globale 'discordWebhook' est vide ou inexistante.");
            return false;
        }

        // 2. RÉCUPÉRATION DES INFOS (Sécurité ContainsKey)
        string user = args.ContainsKey("userName") ? args["userName"].ToString() : "Un spectateur";
        string game = args.ContainsKey("gameName") ? args["gameName"].ToString() : "Jeu non défini";

        // 3. CRÉATION DU CLIP VIA L'API NATIVE
        var clip = CPH.CreateClip();
        
        if (clip == null)
        {
            CPH.SendMessage($"⚠️ @{user}, Twitch n'a pas pu générer le clip (VOD désactivée ou bug).");
            return false;
        }

        // Construction des URLs
        string clipUrl = $"https://clips.twitch.tv/{clip.Id}";
        string thumbUrl = clip.ThumbnailUrl ?? "";

        // 4. TEMPO DE SYNCHRONISATION
        // On attend 2 secondes pour que les serveurs de métadonnées Twitch se mettent à jour
        CPH.Wait(2000);

        // 5. ENVOI CHAT TWITCH
        CPH.SendMessage($"🎬 Nouveau clip ! Merci @{user} : {clipUrl}");

        // 6. ENVOI DISCORD (Méthode optimisée pour l'aperçu vidéo)
        using (HttpClient client = new HttpClient())
        {
            try
            {
                // Note : On met l'URL dans 'content' pour forcer l'embed automatique de Discord
                var discordPayload = new
                {
                    content = $"🎬 **Nouveau Clip de {user} !**\n{clipUrl}",
                    embeds = new[]
                    {
                        new
                        {
                            title = "Détails de la séquence",
                            color = 10181046, // Violet Twitch
                            fields = new[]
                            {
                                new { name = "👤 Créé par", value = user, inline = true },
                                new { name = "🎮 Jeu", value = game, inline = true }
                            },
                            image = new { url = thumbUrl },
                            footer = new { text = "Système Automatique • Gland Économie" },
                            timestamp = DateTime.UtcNow
                        }
                    }
                };

                string json = JsonConvert.SerializeObject(discordPayload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                // Envoi synchrone pour Streamer.bot
                var response = client.PostAsync(webhookUrl, content).Result;
                
                if (!response.IsSuccessStatusCode)
                {
                    CPH.LogDebug($"⚠️ Discord a répondu avec l'erreur : {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                CPH.LogError("❌ Erreur lors de l'envoi au Webhook Discord : " + ex.Message);
            }
        }

        return true;
    }
}
