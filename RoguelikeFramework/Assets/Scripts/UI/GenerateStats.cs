using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GenerateStats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int score = Mathf.Clamp(StatTracking.damageDealt + StatTracking.damageTaken, 0, 5000) + 20 * StatTracking.monstersKilled + 30 * StatTracking.playerLevel + (6000 * (StatTracking.victory ? 1 : 0));
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = $"{(StatTracking.victory ? "Congratulations" : "Better luck next time")}, minion! You {(StatTracking.victory ? "won" : "lost")} as a level {StatTracking.playerLevel} minion on depth {StatTracking.floor}.\n\n"
                  + $"Your run ended after {StatTracking.turnsTaken} global turns, over which you made {StatTracking.numberMoves} individual actions and took {StatTracking.stepsTaken} steps.\n\n"
                  + $"You dealt a total of <color=red>{StatTracking.damageDealt} damage<color=white>, killing {StatTracking.monstersKilled} monsters. In return, they dealth <color=red>{StatTracking.damageTaken} damage<color=white> to you{(StatTracking.victory ? "" : ", <color=red>killing you")}.<color=white>\n\n"
                  + $"{(StatTracking.victory ? "You have fulfilled your duty and made your summoner proud." : "Fear not, for you will be <color=red>summonned again<color=white>.")}\n\n"
                  + $"<align=\"center\">Final Score: <color=green>{score}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
