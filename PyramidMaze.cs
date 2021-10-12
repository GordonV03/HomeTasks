namespace Mazes
{
	public static class PyramidMazeTask
	{
		public static void MoveOut(Robot robot, int width, int height)
		{
			width -= 3;
			while (robot.Finished == false)
			{
				MoveInPyramid(robot, width);
				width -= 4;
				if (robot.Finished == false)
					Move(robot, Direction.Up, 2);
			}
		}

		public static void Move(Robot robot, Direction direction, int stepsCount)
		{
			for (int i = 0; i < stepsCount; i++)
				robot.MoveTo(direction);
		}

		public static void MoveInPyramid(Robot robot, int stepsCount)
		{
			Move(robot, Direction.Right, stepsCount);
			Move(robot, Direction.Up, 2);
			Move(robot, Direction.Left, stepsCount - 2);
		}
	}
}