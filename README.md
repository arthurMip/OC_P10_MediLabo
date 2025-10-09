## Projet MediLabo Solutions

## Prérequis

[.NET 9 SDK](https://dotnet.microsoft.com/fr-fr/download/dotnet/9.0)
installé

[Docker](https://www.docker.com/get-started/)
installé et en fonctionnement

## Exécution avec Docker

```bash
docker-compose up --build  --remove-orphans
```

## Ouvrir l'application

Url: http://localhost:5002/

## Login

Email: user1@medilabo.com
Password: Password123!

## Recommandations d’amélioration Green Code

- Sur Docker, fixer une limite sur le CPU/RAM pour limiter les ressources physiques
- Utiliser du code asynchrone pour de meilleures performances
- Mettre en place un cache pour éviter les traitements répétés.
- Réduire le nombre de services
