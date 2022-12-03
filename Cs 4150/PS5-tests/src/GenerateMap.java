/**
 * Generates maps for PS05 - Algorithms, CS 4150, Spring 2022.
 * 
 * Note that while this generates maps, it does not place mosters on 
 * the map.  I use a second program for monster placement that checks
 * to make sure the resulting maps have non-zero player treasure totals,
 * and that adding another monster can result in a lower player treasure
 * total.
 * 
 * This solution relies on OpenSimplex2S for noise generation.  See
 * https://github.com/KdotJPG/OpenSimplex2/blob/master/java/OpenSimplex2S.java
 * in https://github.com/KdotJPG/OpenSimplex2.
 * 
 * @author Peter Jensen
 * @version March 1, 2022
 */
import java.util.Random;
import java.util.Scanner;

public class GenerateMap
{

	/**
	 * Application entry point
	 * 
	 * @param args not used
	 */
	public static void main(String[] args)
	{
		Scanner in = new Scanner(System.in);
				
		System.out.print("Use what random seed? ");
		long seed = in.nextLong();
		
		System.out.print("How many rows? ");
		int rows = in.nextInt();
		
		System.out.print("How many columns? ");
		int columns = in.nextInt();
		
		System.out.print("Use what zoom for walls (0.2ish)?");
		double wallZoom = in.nextDouble();
		
		System.out.print("Use what threshold for walls (0.2ish)?");
		double wallThreshold = in.nextDouble();
		
		System.out.print("Use what percentage of space for treasure (0.25ish)?");
		double treasurePercentage = in.nextDouble();
		
		char[][] map = generate(seed, columns, rows, wallZoom, wallThreshold, treasurePercentage);
		String mapText = convertMapToInputs(map);
		System.out.println(mapText);		
	}
	
	/**
	 * Converts a map to a string suitable to printing or parsing as a test case for
	 * ps5.
	 * 
	 * @param map a 2D char array
	 * @return a string representation of the map as inputs
	 */
	public static String convertMapToInputs (char[][] map)
	{
		String result = map.length + " " + map[0].length + "\n";
		
		for (int row = 0; row < map.length; row++)
			result += new String(map[row]) + "\n";
		
		return result;
	}
	
	
	/**
	 * Generates a random map, zoomed in to simplex noise (similar to Perlin noise).
	 * 
	 * @param seed  any long
	 * @param width  map width >= 3
	 * @param height map height >= 3
	 * @param wallScale  typical value is 0.2ish
	 * @param wallThreshold typical value is 0.2ish
	 * @param treasurePercentage typical value is 0.2ish
	 * @param treasureThreshold typical value is 0.7ish
	 * @return a map without monsters
	 */
	public static char[][] generate (long seed, int width, int height, double wallScale, double wallThreshold, double treasurePercentage)
	{
		// Make a random number generator (for treasure value generation) based on the seed.
		
		Random rand = new Random(seed);
		
		// Make an empty map
		
		char[][] map = new char[height][width];  // Row, column ordering
		
		// Fill in perimeter.
		
		for (int row = 0; row < height; row++)
			map[row][0] = map[row][width-1] = '#';
		
		for (int col = 1; col < width-1; col++)
			map[0][col] = map[height-1][col] = '#';
		
		// Use noise to select walls or open space.  Noise is in [-1...1] (or so), 
		// convert each noise sample to map spaces.
		//
		// When generating noise, use wallScale and treasureScale to 'zoom in' on the noise.
		// Smaller values = bigger zoom in, smaller values = thicker walls and wider open
		// spaces.
		
		// First, make the walls with the seed unmodified.
		// noise >= wallThreshold, put a wall in the map; otherwise, put open space in map  
		
		int openSpaceCount = 0;
		
		for (int row = 1; row < height-1; row++)
			for (int col = 1; col < width-1; col++)
			{
				double noise = OpenSimplex2S.noise2(seed, col*wallScale, row*wallScale);
				map[row][col] = noise >= wallThreshold ? '#' :'.';
				if (map[row][col] == '.')
					openSpaceCount++;
			}
		
		// Note -- if there are no open spaces, we cannot place a start location.
		
		if (openSpaceCount == 0)
			throw new RuntimeException ("No open spaces for this seed / configuration.");
		
		// Next, place treasure randomly (without the clustering of the noise generator
		// used in placing walls).
		
		for (int row = 1; row < height-1; row++)
			for (int col = 1; col < width-1; col++)
				if (map[row][col] == '.')
				{
					if (rand.nextDouble() <= treasurePercentage)
						map[row][col] = (char)(rand.nextInt(9) + '1');
			}
		
		// Finally, pick a starting location.
		// First, choose randomly a bunch of times.  On success, return the map.
		
		for (int attempt = 1; attempt < 10000; attempt++)
		{
			int row = rand.nextInt(height-2)+1;
			int col = rand.nextInt(width-2)+1;
			
			if (map[row][col] != '#')
			{
				System.out.println("Start placed at " + row + " " + col + ".");
				map[row][col] = 'p';
				return map;
			}
		}
		
		// As a last resort, place the start point in the first available open space.
		
		outer: for (int row = 1; row < height-1; row++)
			for (int col = 1; col < width-1; col++)
				if (map[row][col] != '#')
				{
					System.out.println("Start placed at " + row + " " + col + ".");
					map[row][col] = 'p';
					break outer;
				}
		
		return map;
	}
	
}