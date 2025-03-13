using System;

namespace Game.Source.Models
{
    public class TeamMatrix
    {
        private long _bitMask;

        private readonly int _dimension = typeof(Team).GetEnumValues().Length;

        public long Mask => _bitMask;

        public TeamMatrix()
        {
        }

        public TeamMatrix(long mask)
        {
            SetMask(mask);
        }

        public void SetMask(long mask)
        {
            _bitMask = mask;
        }

        public void SetInteraction(Team team1, Team team2, bool value)
        {
            SetInteraction((int)team1, (int)team2, value);
        }

        public void SetInteraction(int team1, int team2, bool value)
        {
            long interactMask = GetInteractMask(team1, team2);

            if (value)
                _bitMask |= interactMask;
            else
                _bitMask &= ~interactMask;
        }

        public bool IsIntractable(Team team1, Team team2)
        {
            return IsIntractable((int)team1, (int)team2);
        }

        public bool IsIntractable(int team1, int team2)
        {
            long interactMask = GetInteractMask(team1, team2);

            return (interactMask & _bitMask) == interactMask;
        }

        private long GetInteractMask(int team1, int team2)
        {
            (team1, team2) = Normalize(team1, team2);

            return 1L << (team1 * _dimension + team2);
        }

        private (int, int) Normalize(int team1, int team2)
        {
            if (team1 + (_dimension - team2 - 1) >= _dimension)
                (team1, team2) = (team2, team1);

            return (team1, team2);
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();

            sb.AppendLine(Convert.ToString(_bitMask, 2));

            for (int i = 0; i < _dimension; i++)
            {
                for (int j = 0; j < _dimension; j++)
                {
                    sb.Append(IsIntractable(i, j) ? "1\t" : "0\t");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}