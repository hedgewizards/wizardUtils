namespace WizardUtils
{
    public static class RandomHelper
    {
        public static int[] ChooseNFromK(this System.Random random, int n, int k)
        {
            int[] options = new int[k];
            int[] choices = new int[n];
            for (int index = 0; index < k; index++)
            {
                options[index] = index;
            }

            for (int choice = 0; choice < n; choice++)
            {
                var chosenIndex = random.Next(choice, k);
                choices[choice] = options[chosenIndex];

                options[chosenIndex] = options[choice];
            }

            return choices;
        }
    }
}
