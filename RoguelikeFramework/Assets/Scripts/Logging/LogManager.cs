using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    private static List<GameObject> logs = new List<GameObject>();

    [Header("Logger Window Objects and Values")]
    [SerializeField]
    private GameObject loggerWindow;
    [SerializeField]
    private Transform loggerWindowContentArea;
    [SerializeField]
    private int maximumNumMessages = 50;

    [Header("Non-Scene Objects")]
    [SerializeField]
    private GameObject messagePrefab;

    [Header("Color Values")]
    [SerializeField]
    private Color itemHighlightColor = Color.yellow;
    [SerializeField]
    private Color healHighlightColor = Color.green;
    [SerializeField]
    private Color enemyHighlightColor = new Color(255, 102, 0);
    [SerializeField]
    private Color damageDealtHighlightColor = Color.blue;
    [SerializeField]
    private Color damageTakenHighlightColor = Color.red;

    private string itemHighlightColorHexString;
    private string healHighlightColorHexString;
    private string enemyHighlightColorHexString;
    private string damageDealtHighlightColorHexString;
    private string damageTakenHighlightColorHexString;

    public static LogManager S;

    private void Awake()
    {
        if (S == null) S = this;

        // ColorUtility is used to convert the colors to a tag-usable hex string
        itemHighlightColorHexString = ColorUtility.ToHtmlStringRGB(itemHighlightColor);
        healHighlightColorHexString = ColorUtility.ToHtmlStringRGB(healHighlightColor);
        enemyHighlightColorHexString = ColorUtility.ToHtmlStringRGB(enemyHighlightColor);
        damageDealtHighlightColorHexString = ColorUtility.ToHtmlStringRGB(damageDealtHighlightColor);
        damageTakenHighlightColorHexString = ColorUtility.ToHtmlStringRGB(damageTakenHighlightColor);
    }

    /// <summary>
    /// Sends a message to the console and automatically removes older messages past a certain threshold.
    /// </summary>
    /// <param name="msg">The message to send to the console.</param>
    private void Log(string msg)
    {
        // Retrieve and delete the oldest message if we are over the maximum number of messages we should display
        if (logs.Count > maximumNumMessages)
        {
            GameObject oldestMessage = logs[0];
            logs.Remove(oldestMessage);
            Destroy(oldestMessage);
            //logs.RemoveRange(0, logs.Count - 50);
        }

        // Create a new message object and set its text
        GameObject messageObj = Instantiate(messagePrefab, loggerWindowContentArea);
        messageObj.GetComponent<MessageControl>().SetMessageText(msg);
        // Add that message object to the log
        logs.Add(messageObj);
        // Tell the logger to forcibly scroll to the bottom of the window.
        Canvas.ForceUpdateCanvases();
        loggerWindow.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
    }

    ////////// REFORMATTED LOGGING //////////

    /// <summary>
    /// Prints a message to the log that specifies how much damage a given entity took. Use this in combat.
    /// </summary>
    /// <param name="damagePoints">How much damage the entity took.</param>
    /// <param name="entityName">The name of the entity taking damage.</param>
    /// <param name="pluralVerbs">Whether the entity requires plural verbs to be used. Optional; true by default.</param>
    /// <param name="isEnemy">Whether the entity taking damage is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the name of the entity in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogGenericEntityDamage(int damagePoints, string entityName, bool pluralVerbs=true, bool isEnemy=false, bool useCapitals=false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        Log(nameString + $" took <color=#{damageTakenHighlightColorHexString}>{damagePoints} damage</color>.");
    }

    /// <summary>
    /// Prints a message to the log that specifies a specific attack between two entities. Use this in combat. This can also be used for other interactions as well.
    /// </summary>
    /// <param name="attackingEntityName">The name of the entity attacking.</param>
    /// <param name="defendingEntityName">The name of the entity defending.</param>
    /// <param name="verb">The verb used against the entity. Do not make this plural.</param>
    /// <param name="pluralVerbs">Whether the attacking entity requires plural verbs to be used. Optional; true by default.</param>
    /// <param name="attackingEntityIsEnemy">Whether the entity attacking is an enemy. Optional; false by default.</param>
    /// <param name="defendingEntityIsEnemy">Whether the entity defending is an enemy. Optional; true by default.</param>
    /// <param name="useCapitals">Whether to capitalize the names of the entities in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogSpecificEntityAttack(string attackingEntityName, string defendingEntityName, string verb, bool pluralVerbs=true, bool attackingEntityIsEnemy=false, bool defendingEntityIsEnemy=true, bool useCapitals=false)
    {
        string attackingEntityNameString = createHighlightedEntityName((useCapitals ? capitalize(attackingEntityName) : attackingEntityName), attackingEntityIsEnemy, true);
        string defendingEntityNameString = createHighlightedEntityName((useCapitals ? capitalize(defendingEntityName) : defendingEntityName), defendingEntityIsEnemy);
        if (defendingEntityName.ToLower() == attackingEntityName.ToLower() && attackingEntityName.ToLower() == "you")
            defendingEntityNameString = "yourself";
        Log(attackingEntityNameString + $" {verb}{(pluralVerbs ? "s" : "")} " + defendingEntityNameString + ".");
    }

    /// <summary>
    /// Prints a message to the log that specifies a specific attack between two entities and includes damage. Use this in combat.
    /// </summary>
    /// <param name="attackingEntityName">The name of the entity attacking.</param>
    /// <param name="defendingEntityName">The name of the entity defending.</param>
    /// <param name="verb">The verb used against the entity. Do not make this plural.</param>
    /// <param name="damagePoints">How much damage the entity took.</param>
    /// <param name="pluralVerbs">Whether the attacking entity requires plural verbs to be used. Optional; true by default.</param>
    /// <param name="attackingEntityIsEnemy">Whether the entity attacking is an enemy. Optional; false by default.</param>
    /// <param name="defendingEntityIsEnemy">Whether the entity defending is an enemy. Optional; true by default.</param>
    /// <param name="useCapitals">Whether to capitalize the names of the entities in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogSpecificEntityAttackWithDamage(string attackingEntityName, string defendingEntityName, string verb, int damagePoints, bool pluralVerbs = true, bool attackingEntityIsEnemy = false, bool defendingEntityIsEnemy = true, bool useCapitals = false)
    {
        string attackingEntityNameString = createHighlightedEntityName((useCapitals ? capitalize(attackingEntityName) : attackingEntityName), attackingEntityIsEnemy, true);
        string defendingEntityNameString = createHighlightedEntityName((useCapitals ? capitalize(defendingEntityName) : defendingEntityName), defendingEntityIsEnemy);
        if (defendingEntityName.ToLower() == attackingEntityName.ToLower() && attackingEntityName.ToLower() == "you")
            defendingEntityNameString = "yourself";
        Log(attackingEntityNameString + $" {verb}{(pluralVerbs ? "s" : "")} " + defendingEntityNameString + $" for <color=#{damageTakenHighlightColorHexString}>{damagePoints} damage</color>.");
    }

    /// <summary>
    /// Logs an entity healing for some amount of HP. Use this when an entity regains HP.
    /// </summary>
    /// <param name="entityName">The name of the entity healing.</param>
    /// <param name="healPoints">The amount of HP the enemy heals for.</param>
    /// <param name="pluralVerbs">Whether the entity healing requires plural verbs. Optional; true by default.</param>
    /// <param name="isEnemy">Whether the entity healing is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the name of the entity in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogEntityHeal(string entityName, int healPoints, bool pluralVerbs=true, bool isEnemy=false, bool useCapitals=false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        Log(nameString + $" heal{(pluralVerbs ? "s" : "")} <color=#{healHighlightColorHexString}>{healPoints} HP</color>.");
    }

    /// <summary>
    /// Logs a general use of an item by an entity. Use this when interacting with or using an item.
    /// </summary>
    /// <param name="entityName">The name of the entity using the item.</param>
    /// <param name="itemName">The name of the item being used.</param>
    /// <param name="pluralVerbs">Whether the entity using the item requires plural verbs. Optional; true by default.</param>
    /// <param name="isEnemy">Whether the entity using the item is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the names of the entity and the item in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogGenericUse(string entityName, string itemName, bool pluralVerbs=true, bool isEnemy=false, bool useCapitals=false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        Log(nameString + $" use{(pluralVerbs ? "d" : "")} <color=#{itemHighlightColorHexString}>{(useCapitals ? capitalize(itemName) : itemName)}</color>.");
    }

    /// <summary>
    /// Logs a specific use of an item by an entity. Use this when interacting with or using an item in a manner that requires more specificity than the word "use".
    /// </summary>
    /// <param name="entityName">The name of the entity using the item.</param>
    /// <param name="itemName">The name of the item being used.</param>
    /// <param name="verb">The specific verb usage (i.e., "drink"). Do not make this plural.</param>
    /// <param name="pluralVerbs">Whether the entity using the item requires plural verbs. Optional; true by default.</param>
    /// <param name="isEnemy">Whether the entity using the item is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the names of the entity and the item in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogSpecificUse(string entityName, string itemName, string verb, bool pluralVerbs=true, bool isEnemy=false, bool useCapitals=false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        Log(nameString + $" {verb}{(pluralVerbs ? "ed" : "")} <color=#{itemHighlightColorHexString}>{(useCapitals ? capitalize(itemName) : itemName)}</color>.");
    }

    /// <summary>
    /// Logs an entity's death. Use this when an entity dies.
    /// </summary>
    /// <param name="entityName">The name of the entity dying.</param>
    /// <param name="pluralVerbs">Whether the entity dying requires plural verbs. Optional; true by default.</param>
    /// <param name="isEnemy">Whether the entity dying is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the name of the entity in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogSpecificEntityDeath(string entityName, bool pluralVerbs=true, bool isEnemy=false, bool useCapitals=false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        Log(nameString + $" die{(pluralVerbs ? "s" : "")}.");
    }

    /// <summary>
    /// Logs a status on the entity in the form of "Entity is status." or "Entity are status" Use this when you want to communicate a change in an entity's status (confused, on fire, alert, staring at you, etc.); basically any message requiring an "is".
    /// </summary>
    /// <param name="entityName">The name of the entity whose status will be logged.</param>
    /// <param name="status">The status to display. Follows the verb "is" if singular or "are" if plural.</param>
    /// <param name="hexStringOfEffect">The hex string to use with this status to color it; can be used in the case of something like "frozen", "on fire", etc., but otherwise defaults to white. Optional; empty by default.</param>
    /// <param name="pluralVerbs">Whether the entity requires plural verbs. Optional; false by default.</param>
    /// <param name="isEnemy">Whether the entity is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the name of the entity in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogStatusOnEntity(string entityName, string status, string hexStringOfEffect="", bool pluralVerbs=false, bool isEnemy=false, bool useCapitals=false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        string statusString = hexStringOfEffect.Length > 0 ? $"<color=#{hexStringOfEffect}>{status}</color>" : $"{status}";
        Log(nameString + $" {(pluralVerbs ? "are" : "is")} {statusString}.");
    }

    /// <summary>
    /// Logs a status on the entity in the form of "Entity is status" or "Entity are status" with a possible secondary note. Use this when you want to communicate a change in an entity's status (confused, on fire, alert, staring at you, etc.); basically any message requiring an "is", but with an exclamation point or a secondary note.
    /// </summary>
    /// <param name="entityName">The name of the entity whose status will be logged.</param>
    /// <param name="status">The status to display. Follows the verb "is" if singular or "are" if plural.</param>
    /// <param name="secondaryStatus">The secondary status to display. Follows an "and" after the first status. Optional.</param>
    /// <param name="endingPunct">The ending punctuation to use.</param>
    /// <param name="hexStringOfEffect">The hex string to use with this status to color it; can be used in the case of something like "frozen", "on fire", etc., but otherwise defaults to white. Optional; empty by default.</param>
    /// <param name="pluralVerbs">Whether the entity requires plural verbs. Optional; false by default.</param>
    /// <param name="isEnemy">Whether the entity is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the name of the entity in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogStatusOnEntityWithFlair(string entityName, string status, string secondaryStatus = null, string endingPunct = "!", string hexStringOfEffect = "", bool pluralVerbs = false, bool isEnemy = false, bool useCapitals = false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        string statusString = hexStringOfEffect.Length > 0 ? $"<color=#{hexStringOfEffect}>{status}</color>" : $"{status}" + secondaryStatus != "" && secondaryStatus != null ? $" and {secondaryStatus}" : "";
        Log(nameString + $" {(pluralVerbs ? "are" : "is")} {statusString}{endingPunct}");
    }

    /// <summary>
    /// Logs a special status on an entity, where the status also acts as a verb and is fully highlighted.
    /// </summary>
    /// <param name="entityName">The name of the entity whose status will be logged.</param>
    /// <param name="status">The status to display. Follows the verb "is" if singular or "are" if plural.</param>
    /// <param name="endingPunct">The ending punctuation to use.</param>
    /// <param name="hexStringOfEffect"></param>
    /// <param name="pluralVerbs">Whether the entity requires plural verbs. Optional; false by default.</param>
    /// <param name="isEnemy">Whether the entity is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the name of the entity in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogSpecialStatus(string entityName, string status, string endingPunct = "!", string hexStringOfEffect = "", bool pluralVerbs = false, bool isEnemy = false, bool useCapitals = false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        string statusString = hexStringOfEffect.Length > 0 ? $"<color=#{hexStringOfEffect}>{status}</color>" : $"{status}";
        Log(nameString + $" {statusString}{endingPunct}");
    }

    /// <summary>
    /// Logs an item pickup from an entity. Use this when an entity picks something up.
    /// </summary>
    /// <param name="entityName">The name of the entity who picks up an item.</param>
    /// <param name="itemName">The name of the item picked up.</param>
    /// <param name="isEnemy">Whether the entity is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the name of the entity in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogPickup(string entityName, string itemName, bool isEnemy = false, bool useCapitals = false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        Log(nameString + $" picked up <color=#{itemHighlightColorHexString}>{itemName}</color>.");
    }

    /// <summary>
    /// Logs several simultaneous item pickups from an entity. Use this when an entity picks multiple items up.
    /// </summary>
    /// <param name="entityName">The name of the entity who picks up these items.</param>
    /// <param name="itemNames">The names of the items picked up.</param>
    /// <param name="isEnemy">Whether the entity is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the name of the entity in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogPickupMultiple(string entityName, string[] itemNames, bool isEnemy = false, bool useCapitals = false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        if (itemNames.Length == 1) LogPickup(entityName, itemNames[0], isEnemy, useCapitals);
        else
        {
            string combinedNameString;
            if (itemNames.Length == 2) combinedNameString = $"<color=#{itemHighlightColorHexString}>{itemNames[0]}</color> and <color =#{itemHighlightColorHexString}>{itemNames[1]}</color>";
            else
            {
                string[] splitFormattedStrings = String.Join($"</color>, <color=#{itemHighlightColorHexString}>", itemNames).Split(',');
                splitFormattedStrings[splitFormattedStrings.Length - 1] = " and" + splitFormattedStrings[splitFormattedStrings.Length - 1];
                combinedNameString = $"<color=#{itemHighlightColorHexString}>" + String.Join("", splitFormattedStrings);
            }
            Log(nameString + $" picked up {combinedNameString}.");
        }
    }

    /// <summary>
    /// Logs an item drop from an entity. Use this when an entity drops something.
    /// </summary>
    /// <param name="entityName">The name of the entity who dropped an item.</param>
    /// <param name="itemName">The name of the item dropped.</param>
    /// <param name="isEnemy">Whether the entity is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the name of the entity in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogDrop(string entityName, string itemName, bool isEnemy = false, bool useCapitals = false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        Log(nameString + $" dropped <color=#{itemHighlightColorHexString}>{itemName}</color>.");
    }

    /// <summary>
    /// Logs several simultaneous item drops from an entity. Use this when an entity drops multiple items.
    /// </summary>
    /// <param name="entityName">The name of the entity who dropped items.</param>
    /// <param name="itemName">The names of the items dropped.</param>
    /// <param name="isEnemy">Whether the entity is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the name of the entity in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogDropMultiple(string entityName, string[] itemNames, bool isEnemy = false, bool useCapitals = false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        if (itemNames.Length == 1) LogDrop(entityName, itemNames[0], isEnemy, useCapitals);
        else
        {
            string combinedNameString;
            if (itemNames.Length == 2) combinedNameString = $"<color=#{itemHighlightColorHexString}>{itemNames[0]}</color> and <color =#{itemHighlightColorHexString}>{itemNames[1]}</color>";
            else
            {
                string[] splitFormattedStrings = String.Join($"</color>, <color=#{itemHighlightColorHexString}>", itemNames).Split(',');
                splitFormattedStrings[splitFormattedStrings.Length - 1] = " and" + splitFormattedStrings[splitFormattedStrings.Length - 1];
                combinedNameString = $"<color=#{itemHighlightColorHexString}>" + String.Join("", splitFormattedStrings);
            }
            Log(nameString + $" dropped {combinedNameString}.");
        }
    }

    /// <summary>
    /// Logs a simulteneous item pickup and item drop. Use this when an entity equips an item and drops another one at the same time.
    /// </summary>
    /// <param name="entityName">The name of the entity who picked up and dropped items.</param>
    /// <param name="pickedUpItemName">The name of the item picked up.</param>
    /// <param name="droppedItemName">The name of the item dropped.</param>
    /// <param name="isEnemy">Whether the entity is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the name of the entity in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogPickupAndDrop(string entityName, string pickedUpItemName, string droppedItemName, bool isEnemy = false, bool useCapitals = false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        Log(nameString + $" picked up <color=#{itemHighlightColorHexString}>{pickedUpItemName}</color> and dropped <color=#{itemHighlightColorHexString}>{droppedItemName}</color>.");
    }


    /// <summary>
    /// Logs an item equip or unequip by an entity. Use this when an entity equips or unequips an item.
    /// </summary>
    /// <param name="entityName">The name of the entity who equipped or unequipped an item.</param>
    /// <param name="itemName">The name of the item equipped or unequipped.</param>
    /// <param name="unequip">Whether the item is being unequipped instad of equipped. Optional; false by default.</param>
    /// <param name="isEnemy">Whether the entity is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the name of the entity in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogEquipOrUnequip(string entityName, string itemName, bool unequip = false, bool isEnemy = false, bool useCapitals = false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        Log(nameString + $" {(unequip ? "un" : "")}equipped <color=#{itemHighlightColorHexString}>{itemName}</color>.");
    }

    /// <summary>
    /// Logs several simultaneous item equips or unequips by an entity. Use this when an entity equips or unequips several items at once.
    /// </summary>
    /// <param name="entityName">The name of the entity who equipped or unequipped these items.</param>
    /// <param name="itemName">The names of the items equipped or unequipped.</param>
    /// <param name="unequip">Whether the items are being unequipped instad of equipped. Optional; false by default.</param>
    /// <param name="isEnemy">Whether the entity is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the name of the entity in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogEquipOrUnequipMultiple(string entityName, string[] itemNames, bool unequip = false, bool isEnemy = false, bool useCapitals = false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        if (itemNames.Length == 1) LogEquipOrUnequip(entityName, itemNames[0], unequip, isEnemy, useCapitals);
        else
        {
            string combinedNameString;
            if (itemNames.Length == 2) combinedNameString = $"<color=#{itemHighlightColorHexString}>{itemNames[0]}</color> and <color =#{itemHighlightColorHexString}>{itemNames[1]}</color>";
            else
            {
                string[] splitFormattedStrings = String.Join($"</color>, <color=#{itemHighlightColorHexString}>", itemNames).Split(',');
                splitFormattedStrings[splitFormattedStrings.Length - 1] = " and" + splitFormattedStrings[splitFormattedStrings.Length - 1];
                combinedNameString = $"<color=#{itemHighlightColorHexString}>" + String.Join("", splitFormattedStrings);
            }
            Log(nameString + $" {(unequip ? "un" : "")}equipped {combinedNameString}.");
        }
    }

    /// <summary>
    /// Logs a simultaneous item equip and separate item unequip. Use this when the player equips an item and unequips an item at the same time.
    /// </summary>
    /// <param name="entityName">The name of the entity who equipped and unequipped these items.</param>
    /// <param name="equippedItemName">The name of the equipped item.</param>
    /// <param name="unequippedItemName">The name of the unequipped item.</param>
    /// <param name="equipBodyPart">The name of the body part the equipped item was equipped to. Optional; null by default.</param>
    /// <param name="unequipBodyPart">The name of the body part the unequipped item was unequipped from. Optional; null by default.</param>
    /// <param name="isEnemy">Whether the entity is an enemy. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the name of the entity in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogEquipAndUnequip(string entityName, string equippedItemName, string unequippedItemName, string equipBodyPart = null, string unequipBodyPart = null, bool isEnemy = false, bool useCapitals = false)
    {
        string nameString = createHighlightedEntityName((useCapitals ? capitalize(entityName) : entityName), isEnemy, true);
        Log(nameString + $" equipped <color=#{itemHighlightColorHexString}>{equippedItemName}</color>{(equipBodyPart == null ? "" : " to " + equipBodyPart)} and unequipped <color=#{itemHighlightColorHexString}>{unequippedItemName}</color>{(unequipBodyPart == null || unequipBodyPart.Equals(equipBodyPart) ? "" : " to " + unequipBodyPart)}.");
    }

    /// <summary>
    /// Logs a quote, optionally as spoken from an entity or an item.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="messageColor">The color of the message. Optional; red by default.</param>
    /// <param name="speakerName">The name of the speaker. Optional; empty by default.</param>
    /// <param name="isItem">Whether the speaker is an item. Optional; false by default.</param>
    /// <param name="isEnemy">Whether the speaker is an item. Optional; false by default.</param>
    /// <param name="useCapitals">Whether to capitalize the name of the entity in this message. Optional; false by default. Do not enable this unless the logger uses both capitalized and lowercase letters, or it will slow the game down.</param>
    public void LogTotallyHighlightedQuote(string message, string messageColor="FF0000", string speakerName="", bool isItem = false, bool isEnemy = false, bool useCapitals = false)
    {
        if (speakerName.Length == 0)
        {
            string highlightedMessage = $"<color=#{messageColor}>{message}";
            Log(highlightedMessage);
        }
        else {
            string nameString = createHighlightedEntityName((useCapitals ? capitalize(speakerName) : speakerName), isEnemy, true, isItem);
            Log(nameString + $": <color=#{messageColor}>{message}");
        }
    }

    ///// HELPER METHODS /////

    private string createHighlightedEntityName(string name, bool isEnemy, bool isFirstWord = false, bool isItem = false)
    {
        if (isFirstWord) name = capitalize(name);
        return $"<color=#{(isEnemy ? enemyHighlightColorHexString : (isItem? itemHighlightColorHexString : "FFFFFF"))}>{(name.ToLower() == "you" && !isFirstWord ? "you" : capitalize(name, true))}</color>";
    }

    /// <summary>
    /// Automatically capitalizes the starting word or all words within the given string.
    /// </summary>
    /// <param name="words">The word string to capitalize.</param>
    /// <param name="capitalizeAllWords">Whether to capitalize all words in the string, or just the first one.</param>
    /// <returns></returns>
    private string capitalize(string words, bool capitalizeAllWords=false)
    {
        // Set of words not to capitalize - articles, conjunctions, and prepositions
        HashSet<string> wordsNotToCapitalize = new HashSet<string> { "a", "an", "the", "for", "and", "nor", "but", "or", "yet", "so", "at", "around", "by", "after", "along", "for", "from", "of", "on", "to", "with", "without" };

        if (words.Length == 0) return "";

        string capitalizedWords = "";

        // Capitalize all words
        if (capitalizeAllWords)
        {
            string[] wordsArr = words.Split(' ');

            foreach (string word in wordsArr)
            {
                if (wordsNotToCapitalize.Contains(word))
                {
                    capitalizedWords += word + " ";
                }
                else
                {
                    capitalizedWords += word.Substring(0, 1).ToUpper();
                    capitalizedWords += (word.Length > 1 ? word.Substring(1) : "") + " ";
                }
            }
        }
        // Capitalize only the first word
        else
        {
            capitalizedWords += words.Substring(0, 1).ToUpper();
            capitalizedWords += (words.Length > 1 ? words.Substring(1) : "") + " ";
        }

        return capitalizedWords.Trim();
    }

    // All clear (we probably won't use it); might not work yet
    /// <summary>
    /// Clears all messages from the log window.
    /// </summary>
    public static void ClearAllMessages()
    {
        logs.Clear();
    }


    ////////// OLD LOGGING, DO NOT USE //////////
    /*
    /// <summary>
    /// General function that tells the player they have done something or something has happened to them in the logger window.
    /// </summary>
    /// <param name="msg">The message to tell the player.</param>
    public void LogYou(string msg)
    {
        Log($"You {msg}.");
    }

    /// <summary>
    /// Tells the player they have healed a certain amount of points in the logger window.
    /// </summary>
    /// <param name="healPoints">The amount of HP the player has healed.</param>
    public void LogHeal(int healPoints)
    {
        LogYou($"regained <color=#{healHighlightColorHexString}>{healPoints} HP</color>");
    }

    /// <summary>
    /// Tells the player they have picked up a certain item in the logger window.
    /// </summary>
    /// <param name="item">The item that the player just picked up.</param>
    public void LogPickup(string item)
    {
        LogYou($"picked up <color=#{itemHighlightColorHexString}>{item}</color>");
    }

    /// <summary>
    /// Tells the player they have obtained a certain item in the logger window.
    /// </summary>
    /// <param name="item">The item that the player just obtained.</param>
    public void LogObtain(string item)
    {
        LogYou($"obtained the <color=#{itemHighlightColorHexString}>{item}</color>");
    }

    /// <summary>
    /// Tells the player thay they have used an item in the logger window.
    /// </summary>
    /// <param name="item">The item the player used.</param>
    public void LogUse(string item)
    {
        LogYou($"used the <color=#{itemHighlightColorHexString}>{item}</color>");
    }

    /// <summary>
    /// Tells the player they have taken a certain amount of damage in the logger window.
    /// </summary>
    /// <param name="damagePoints">The amount of damage the player has taken.</param>
    public void LogDamage(int damagePoints)
    {
        LogYou($"took <color=#{damageTakenHighlightColorHexString}>{damagePoints}</color> damage");
    }

    /// <summary>
    /// Tells the player a specific enemy has dealt a specific amount to damage to them in the logger window.
    /// </summary>
    /// <param name="enemyName">The name of the enemy that has hit the player.</param>
    /// <param name="damagePoints">The amount of damage the enemy has dealt.</param>
    public void LogDamageFromEnemy(string enemyName, int damagePoints)
    {
        Log($"The <color=#{enemyHighlightColorHexString}>{enemyName}</color> dealt <color=#{damageTakenHighlightColorHexString}>{damagePoints}</color> damage.");
    }

    /// <summary>
    /// Tells the player how much damage they have dealt to a specific enemy in the logger window.
    /// </summary>
    /// <param name="enemyName">The name of the enemy the player has attacked.</param>
    /// <param name="damagePoints">The amount of damage the player has dealt to the enemy.</param>
    public void LogDamageDealtToEnemy(string enemyName, int damagePoints)
    {
        LogYou($"dealt <color=#{damageDealtHighlightColorHexString}>{damagePoints}</color> to the <color=#{enemyHighlightColorHexString}>{enemyName}</color>");
    }

    /// <summary>
    /// Tells the player they have defeated a certain monster in the log window.
    /// </summary>
    /// <param name="enemyName">The name of the enemy that has been defeated.</param>
    public void LogEnemyDefeat(string enemyName)
    {
        LogYou($"defeated the <color=#{enemyHighlightColorHexString}>{enemyName}</color>");
    }

    /// <summary>
    /// Tells the player they have died in the log window.
    /// </summary>
    public void LogDeath()
    {
        LogYou("died");
    }
    */
}
