using Word2vec.Tools;
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace codenames_solver
{
    public class SimilarityCodewordsScore
    {
        protected const double ASSASSIN_PENALTY = -200;

        protected const int CURRENT_BONUS = 500;
        protected const double NEUTRAL_PENALTY = -50;
        protected const double OPPOSING_PENALTY = -100;

        protected readonly Vocabulary? _vocabulary;

        protected double CosineSimilarity(string word1, string word2)
        {
            if (_vocabulary is null || !_vocabulary.ContainsWord(word1) || !_vocabulary.ContainsWord(word2))
            {
                throw new Exception("One or both words not found in vocabulary");
            }

            float[] vector1 = _vocabulary.GetRepresentationFor(word1).NumericVector;
            float[] vector2 = _vocabulary.GetRepresentationFor(word2).NumericVector;

            double dotProduct = 0.0;
            double norm1 = 0.0;
            double norm2 = 0.0;
            object locker = new();

            Parallel.For(0, _vocabulary.VectorDimensionsCount, i =>
            {
                double tmpDot = vector1[i] * vector2[i];
                double tmpNorm1 = Math.Pow(vector1[i], 2);
                double tmpNorm2 = Math.Pow(vector2[i], 2);

                lock (locker)
                {
                    dotProduct += tmpDot;
                    norm1 += tmpNorm1;
                    norm2 += tmpNorm2;
                }
            });

            return dotProduct / (Math.Sqrt(norm1) * Math.Sqrt(norm2));
        }
    }
}
