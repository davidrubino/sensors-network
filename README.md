Structure des fichiers
======================

 * AppliCliente/ : interface graphique (solution Visual Studio 2013)
 * AppliEmbarquee/ : 
    * SDK/ : SDK allégé contenant les fichiers nécessaires pour compiler les programmes (Z-Stack Home)
    * Common/ : structures communes entre programme coordinateur & capteur/actionneur
    * Coordinator/ : programme coordinateur
    * Sensor/ : programme capteur/actionneur
    * Workspace.eww : workspace IAR contenant les projets Coordinator et Sensor


Interface graphique
===================

L'interface graphique est réalisée sous Visual Studio 2013 à l'aide des Windows Forms.

Au démarrage, les ports séries disponibles sont scannés en envoyant une commande PING, si une réponse est reçue alors ce port est utilisé pour la suite. Si aucun des ports n'a répondu, un message d'erreur sera affiché et l'application proposera de recommencer le scan des ports.

Ensuite, une synchronisation est faite :

 - on synchronise les horloges : on envoie la commande GET_TIME pour savoir depuis combien de secondes le coordinateur a démarré. Cela permet de calculer de manière absolue la date de dernière activité d'un capteur ou actionneur
 - on ajoute les capteurs connus de l'interface et non présents sur le coordinateur dans ce dernier
 - on met à jour depuis le coordinateur les informations sur les capteurs déjà connus de l'interface (type, seuil, etc.)
 - on ajoute les règles connues de l'interface mais pas du coordinateur à ce dernier
 - on ajoute les règles connues du coordinateur mais pas de l'interface à cette dernière. Les noms des règles sont déterminés à partir de l'ID de la règle
 

Enfin, on démarre le thread de mise à jour des données, qui met à jour les capteurs et les règles (dans la classe Communication) et émet l'évènement SensorsChanged.

Cet événement est traité par la méthode *com_SensorsChanged* de *FrmMain* qui met à jour l'état actuel des capteurs à partir de celui du coordinateur.

Bugs connus
-----------
 - Il est possible de créer une même règle plusieurs fois : cela ne pose cependant aucun dysfonctionnement.
 - Il est possible de créer **manuellement (en saisissant l'adresse)** un capteur (ou actionneur) plusieurs fois : l'interface affichera plusieurs capteurs avec la même adresse alors que le coordinateur ne prendra en compte que le dernier capteur ou actionneur créé.
 - La liste des règles est mise à jour à chaque exécution du thread dans la classe Communication, mais l'interface graphique ne prend pas en compte ces nouvelles données. À savoir que la liste des règles n'est pas censée être différente de celle connue par l'interface lors d'une utilisation normale.
 - L'horloge du coordinateur dévie assez rapidement (on l'a vu pendant la soutenance !). Il faudrait la synchroniser périodiquement plutôt qu'uniquement au démarrage.
 - Dans *CreateOrModifyComponentForm*, le nom saisi est effacé si on modifie l'adresse du capteur/actionneur.
 - Le programme plante si l'on débranche le coordinateur. Cela est dû au fait que certaines exceptions ne sont pas gérées lors de l'écriture et de la lecture sur le port série.

Application embarquée
=====================

Structure du code
-----------------

 - *Coordinator.c* : c'est ici où se trouve le code qui répond aux différents événements extérieurs (réception d'une commande, exécution d'un timer, etc.)
 - *Utils.h/Utils.c* : contient l'implémentation des différentes commandes envoyées par l'interface et les fonctions qui traitent la réception d'un message.
 - *NvList.h/NvList.c* : implémentation des listes chaînées stockées en NVRAM

La Z-Stack comprend plusieurs options de compilation sous forme. Ces options de compilation peuvent être changées dans les paramètres des projets (Options, Section *C/C++ compiler*, onglet *Preprocessor*), ainsi que dans les fichiers .cfg (dossier *Tools* dans l'arbre du projet). Attention, ces fichiers sont intégrés directement dans le SDK et non dans le dossier du projet. Il existe un fichier pour chaque type de configuration (f8wEndev.cfg, f8wCoord.cfg, f8wRouter.cfg) ainsi qu'un fichier commun (f8wCommon.cfg).

Par convention, une option est désactivée en ajoutant un *x* devant son nom.

Options de compilation (coordinateur)
-------------------------------------

Le projet *Coordinator* comprend une seule configuration (Coordinator) avec pour options de compilatiion spécifiques :

 - **SECURE=1** : active la sécurité du réseau
 - **NV_INIT** et **NV_RESTORE** : permet de conserver la configuration du réseau lors des redémarrages.
 - **xZTOOL_P1** : désactive l'utilisation du port série par le composant *MT* de *Z-Stack* qui permet d'utiliser l'application *Z-Tool* pour envoyer et recevoir des commandes. En effet, nous utilisons directement le port série.
 - **xMT_TASK, xMT_APP_FUNC, xMT_SYS_FUNC, xMT_SYS_FUNC** : désactive les fonctions de la couche MT qui n'est pas utilisée.
 - **LCD_SUPPORTED** : active le debug sur l'afficheur LCD pour la Z-Stack

Se reporter au fichier f8wCoord.cfg pour le reste.

Options de compilation (Sensor)
-------------------------------
Le projet *Sensor* comprend les deux configurations (Router & End-Device). Elles ne diffèrent que par l'utilisation du fichier de configuration f8wRouter.cfg ou fwEndev.cfg en fonction du projet. Les options sont documentées dans ces fichiers.

Bugs connus
-----------
 - la fonction *evaluate_triggers()* ne fait pas de OU logique entre plusieurs règles qui concernent un même actionneur. Actuellement, cette fonction évalue chaque règle et active ou désactive l'actionneur concerné si besoin. Il faudrait évaluer au moins toutes les règles concernant un même actionneur, faire un OU logique du résultat de ces règles, puis activer ou désactiver l'actionneur si besoin.

Problèmes éventuels
-------------------
 - Chaque mise à jour d'un capteur entraîne une écriture dans la NVRAM (Flash). Il faudrait stocker ces données en RAM (et conserver en Flash uniquement les paramètres par exemple).
 - Les lectures et écritures du port série se font en un seul coup. Il pourrait y avoir des problèmes de dépassement de buffer si il y a un nombre important de données à envoyer/recevoir.

Conseil pour compiler
---------------------
Il peut être nécessaire, dans certains cas, de devoir tout recompiler, même si l'ide ne l'indique pas, pour que le programme fonctionne correctement. Cela est déjà arrivé notamment lorqu'on passe de la configuration *end-device* à *router*.

Il est souvent nécessaire d'effacer la *NVRAM* avant d'envoyer un programme sur une carte, surtout si l'ancien programme est différent du nouveau (configuration différente).
