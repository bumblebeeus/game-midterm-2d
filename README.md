# Unity2d midterm project

[![Github tag](https://badgen.net/github/tag/bumblebeeus/game-midterm-2d)](https://github.com/bumblebeeus/game-midterm-2d/tags/)

![example workflow](https://github.com/bumblebeeus/game-midterm-2d/actions/workflows/main.yml/badge.svg)

> [!NOTE]  
> This project is in progress.

> [!IMPORTANT]  
> This project belongs to group Bumblebee studying Game Development Course in HCMUS.


# Database connection for local testing

- Step 1: Install **Apache2** in Ubuntu. Follow all these tutorials: [here](https://www.digitalocean.com/community/tutorials/how-to-create-a-self-signed-ssl-certificate-for-apache-in-ubuntu-22-04). <br>
You should change `your_domain_or_ip` to `localhost`.
- Step 2: Install **phpmyadmin** and **mysql** in Ubuntu. Follow this tutorial: [here](https://www.digitalocean.com/community/tutorials/how-to-install-and-secure-phpmyadmin-on-ubuntu-20-04). <br>

- Step 3: (Config webservice) Copy all code of webservice to `/var/www/localhost`.
- Step 4: (Config database) Copy the database file and import into phpMyAdmin.<br>
- Step 5: (Config environment) Change the environment in the file `Assets/Scripts/Configs/Environment.json` to `dev`.<br>

Additional information:
- Code with administrator privileges: `sudo code . --user-data-dir='.' --no-sandbox`.


# Webservice error code

| Error Code | Description |
| --- | --- |
| E001 | DB connection error |
| E002 | DB query cannot be executed |   
| E003 | Query expected to get 1 record only  |
| E004 | Receive update query but request does not provide column to update |
| E005 | Runtime error while execute query in DB |