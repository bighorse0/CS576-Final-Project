using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class TrafficLightsTrigger: MonoBehaviour
{
    public int hardness;
    public GameObject red_light;
    public GameObject green_light;
    public GameObject yellow_light;
    public float bubble_spawn_dist;


    string IntToOperator(int operator_num)
    {
        string operation = "";

        switch (operator_num)
        {
            case 0:
                operation = "+";
                break;
            case 1:
                operation = "-";
                break;
            case 2:
                operation = "*";
                break;
            case 3:
                operation = "/";
                break;
        }

        return operation;
    }

    Dictionary<string, string> GenProbWithRangeAndOperator(int range, int operation)
    {
        Dictionary<string, string> prob_and_sols = new Dictionary<string, string>();

        int operand_one = Random.Range(0, range);
        int operand_two = Random.Range(0, range);
        int correct_answer = 0;
        int incorrect_answer = 0;
        string problem = "";

        switch (operation)
        {
            case 0:
                // Addition
                correct_answer = operand_one + operand_two;
                incorrect_answer = Random.Range(0, 2 * correct_answer);
                if (incorrect_answer == correct_answer)
                {
                    incorrect_answer++;
                }

                problem = operand_one.ToString() + " + " + operand_two.ToString() + " = ?";
                break;
            case 1:
                // Subtraction
                correct_answer = operand_one - operand_two;
                incorrect_answer = Random.Range(2 * correct_answer, -2 * correct_answer);
                if (incorrect_answer == correct_answer)
                {
                    incorrect_answer++;
                }

                problem = operand_one.ToString() + " - " + operand_two.ToString() + " = ?";
                break;
            case 2:
                // Multiplication
                if (operand_one % 10 == 0 || range == 10)
                {
                    operand_two = Random.Range(0, range);
                }
                else
                {
                    int[] possible_mults = new int[] { 0, 1, 2, 10, 100, range };
                    operand_two = possible_mults[Random.Range(0, 5)];
                }
                correct_answer = operand_one * operand_two;

                int[] possible_incorrect_answers = new int[] {
                    correct_answer - 10,
                    correct_answer + 10,
                    correct_answer * 2,
                    correct_answer * 10,
                    correct_answer / 2,
                    correct_answer / 10
                };
                incorrect_answer = possible_incorrect_answers[Random.Range(0, 11)];

                problem = operand_one.ToString() + " * " + operand_two.ToString() + " = ?";
                break;
            case 3:
                // Division
                operand_two = Random.Range(1, range);
                while (operand_one % operand_two != 0)
                {
                    operand_one = Random.Range(0, range);
                    operand_two = Random.Range(1, range);
                }
                correct_answer = (operand_one / operand_two);
                incorrect_answer = Random.Range(0, 2 * correct_answer);
                if (incorrect_answer == correct_answer)
                {
                    incorrect_answer++;
                }

                problem = operand_one.ToString() + " / " + operand_two.ToString() + " = ?";
                break;
        }

        prob_and_sols.Add("problem", problem);
        prob_and_sols.Add("correct_answer", correct_answer.ToString());
        prob_and_sols.Add("incorrect_answer", incorrect_answer.ToString());

        return prob_and_sols;
    }

    Dictionary<string, string> GenerateProblem()
    {

        switch (hardness)
        {
            case 1:
                return GenProbWithRangeAndOperator(21, 0);
            case 2:
                return GenProbWithRangeAndOperator(21, 1);
            case 3:
                return GenProbWithRangeAndOperator(21, 2);
            case 4:
                return GenProbWithRangeAndOperator(21, 3);
            case 5:
                return GenProbWithRangeAndOperator(21, Random.Range(0, 3));
            default:
                return GenProbWithRangeAndOperator(0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("plane"))
        {
            Debug.Log("Plane Collision");

            Dictionary<string, string> prob_and_sols = GenerateProblem();

            Vector3 problem_bubble_pos = other.transform.position;
            problem_bubble_pos.z += bubble_spawn_dist;
            problem_bubble_pos.y = Mathf.Max(problem_bubble_pos.y - 6.0f, 6.0f);

            Vector3 correct_bubble_pos = problem_bubble_pos;
            Vector3 incorrect_bubble_pos = correct_bubble_pos;

            GameObject correct_bubble_prefab;
            GameObject incorrect_bubble_prefab;

            int is_correct_bubble_on_right = Random.Range(0, 2);

            correct_bubble_pos.x += (float)is_correct_bubble_on_right * 12.0f - 6.0f;
            correct_bubble_prefab = is_correct_bubble_on_right == 1 ? red_light : green_light;
            incorrect_bubble_pos.x -= (float)is_correct_bubble_on_right * 12.0f - 6.0f;
            incorrect_bubble_prefab = is_correct_bubble_on_right == 1 ? green_light : red_light;

            GameObject problem_bubble = Instantiate(yellow_light, problem_bubble_pos, Quaternion.identity);
            problem_bubble.name = "PROBLEM_BUBBLE";
            problem_bubble.GetComponent<BubbleHandler>().SetType(0);
            problem_bubble.GetComponent<BubbleHandler>().SetText(prob_and_sols["problem"]);

            GameObject correct_bubble = Instantiate(correct_bubble_prefab, correct_bubble_pos, Quaternion.identity);
            correct_bubble.name = "CORRECT_BUBBLE";
            correct_bubble.GetComponent<BubbleHandler>().SetType(1);
            correct_bubble.GetComponent<BubbleHandler>().SetText(prob_and_sols["correct_answer"]);

            GameObject incorrect_bubble = Instantiate(incorrect_bubble_prefab, incorrect_bubble_pos, Quaternion.identity);
            incorrect_bubble.name = "INCORRECT_BUBBLE";
            incorrect_bubble.GetComponent<BubbleHandler>().SetType(2);
            incorrect_bubble.GetComponent<BubbleHandler>().SetText(prob_and_sols["incorrect_answer"]);


        }
    }
}

