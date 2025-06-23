#!/bin/bash

# 🚀 Script de déploiement pour Blazor Server

# === CONFIGURATION ===
SERVER="root@217.154.113.66"
REMOTE_PATH="/var/www/h-luxury-driving"
DLL_NAME="HLuxuryDriving-Server.dll"
SERVICE_NAME="kestrel-hluxury.service"

echo "📦 Publication de l'application .NET..."
dotnet publish -c Release -o ./publish
if [ $? -ne 0 ]; then
    echo "❌ Échec de la publication."
    exit 1
fi

echo "📡 Transfert des fichiers vers le serveur..."
scp -r ./publish/* "$SERVER:$REMOTE_PATH/"
if [ $? -ne 0 ]; then
    echo "❌ Échec du transfert des fichiers."
    exit 1
fi

echo "🔄 Redémarrage du service sur le VPS..."
ssh $SERVER "systemctl restart $SERVICE_NAME"
if [ $? -ne 0 ]; then
    echo "❌ Échec du redémarrage du service."
    exit 1
fi

echo "✅ Déploiement terminé avec succès !"
