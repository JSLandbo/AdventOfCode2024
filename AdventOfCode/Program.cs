await Solver.SolveAll(opt =>
{
    opt.ShowConstructorElapsedTime = false;
    opt.ShowTotalElapsedTimePerDay = false;
    opt.ElapsedTimeFormatSpecifier = "F3";
});