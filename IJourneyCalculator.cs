namespace Swift
{
    public interface IJourneyCalculator 
    {
        double CalculateTimeFromDepot(Coordinate position);
        double CalculateTimeToCompleteDelivery(Coordinate position1, Coordinate position2);
    }
}