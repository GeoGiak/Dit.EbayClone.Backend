namespace Dit.EbayClone.Backend.Application.Recommendations;

public class MatrixFactorizationAlgorithm
{
    public double[,] U; // users x k
    public double[,] V; // auctions x k
    private int k;
    private double learningRate;
    private double lambda;

    public MatrixFactorizationAlgorithm(
        int numUsers,
        int numActions,
        int latentFeatures = 5,
        double lr = 0.01,
        double reg = 0.1)
    {
        k = latentFeatures;
        learningRate = lr;
        lambda = reg;
        var rand = new Random();

        U = new double[numUsers, k];
        V = new double[numActions, k];
        
        // Initialize with small random values
        for (int i = 0; i < numUsers; i++)
        {
            for (int j = 0; j < k; j++)
            {
                U[i, j] = rand.NextDouble() * 0.1;
            }
        }

        for (int i = 0; i < numActions; i++)
        {
            for (int j = 0; j < k; j++)
            {
                V[i, j] = rand.NextDouble() * 0.1;
            }
        }
    }

    public void Train(List<RecommendationArguments> interactions, int epochs = 50)
    {
        for (int epoch = 0; epoch < epochs; epoch++)
        {
            foreach (var interaction in interactions)
            {
                var predicted = 0.0;
                for (int f = 0; f < k; f++)
                {
                    predicted += U[interaction.userIdx, f] * V[interaction.auctionIdx, f];
                }
                
                var e = interaction.rating - predicted;
                
                // Update latent factors
                for (int f = 0; f < k; f++)
                {
                    var uf = U[interaction.userIdx, f];
                    var vf = V[interaction.auctionIdx, f];
                    
                    U[interaction.userIdx, f] += learningRate * (e * vf - lambda * uf);
                    V[interaction.auctionIdx, f] += learningRate * (e * uf - lambda * vf);
                }
            }
        }
    }

    public double Predict(int userIndex, int auctionIndex)
    {
        var result = 0.0;

        for (var f = 0; f < k; f++)
        {
            result += U[userIndex, f] * V[auctionIndex, f];
        }

        return result;
    }
    
}