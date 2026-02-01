# 🎬 Twitch Clip to Discord (Streamer.bot C#)

Ce script pour **Streamer.bot** permet de créer un clip Twitch instantanément via une commande chat (ex: `!clip`), de récupérer automatiquement les informations du jeu en cours, et d'envoyer le tout proprement sur votre serveur **Discord** via un Webhook.

---

## ✨ Fonctionnalités
* **Automatique** : Crée le clip et récupère l'URL sans action manuelle.
* **Info Jeu** : Affiche le nom du jeu actuel (ex: ARC Raiders).
* **Discord Ready** : Envoie un message formaté sur Discord avec aperçu vidéo.
* **Anti-Fail** : Inclut une boucle de sécurité pour attendre que Twitch génère l'URL.

---

## ⚙️ Configuration dans Streamer.bot

### 1. La Variable Globale (Webhook)
Pour ne pas exposer votre lien Discord dans le code, nous utilisons une variable globale :
1.  Allez dans l'onglet **Settings** > **Globals**.
2.  Ajoutez une nouvelle variable :
    * **Name** : `discordWebhook`
    * **Value** : `VOTRE_URL_WEBHOOK_DISCORD`
3.  Vérifiez que **Persist** est coché.

### 2. Création de l'Action
Créez une nouvelle Action (ex: `Twitch Clip to Discord`) et ajoutez ces **Sub-Actions** dans l'ordre exact :

1.  **Twitch > Clips > Create Clip** (Cochez *Use Broadcaster Account*).
2.  **Twitch > Channel > Get user info for Targer -> %broadcastUser%** (Indispensable pour le nom du jeu).
3.  **Core > C# > Execute Code** :
    * Ouvrez la fenêtre de code.
    * Allez dans l'onglet **References**, clic-droit > **Add reference from file**.
    * Cherchez et ajoutez `System.Net.Http.dll` (souvent dans le dossier de Streamer.bot ou Windows).
    * Copiez-collez le contenu du fichier `ClipToDiscord.cs` présent dans ce dépôt.
    * Cliquez sur **Save and Compile**.

### 3. Le Trigger (Déclencheur)
1.  Allez dans l'onglet **Commands**.
2.  Créez une commande `!clip`.
3.  Liez cette commande à l'action que vous venez de créer.

---

## 📖 Utilisation en Live
Tapez simplement **!clip** dans votre chat Twitch. 
* Le bot va tenter de créer le clip.
* Il attend que l'URL soit disponible.
* Il poste le lien dans votre chat.
* Il envoie l'alerte sur votre Discord.

---

## 🛠️ Dépannage
* **Jeu Inconnu** : Assurez-vous d'avoir bien mis la sub-action `Get user info for Targer -> %broadcastUser%` **AVANT** le code C#.
* **Le clip ne se crée pas** : Twitch limite parfois la création de clips (stream trop récent ou trop de clips en peu de temps).
* **Erreur de compilation** : Vérifiez que vous avez bien ajouté la référence `System.Net.Http.dll`.

---

## 📄 Licence
Distribué sous la licence MIT. Voir `LICENSE` pour plus d'informations.

---
*Développé pour la communauté par [Geek_Des_Bois](https://www.twitch.tv/geek_des_bois)*
