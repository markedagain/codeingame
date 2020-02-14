#include <stdlib.h>
#include <stdio.h>
#include <string.h>

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 * ---
 * Hint: You can use the debug stream to print initialTX and initialTY, if Thor seems not follow your orders.
 **/

//char pScanBuffer[200];
int currentIndex = 0;
const char *data[2];
const int IDE =1 ;

void populate(){
    data[0]="0 17 31 4";
    data[1]="44";
    data[2]="43";
    data[3]="42";
    data[4]="41";
    data[5]="40";
    data[6]="39";
    data[7]="38";
    data[8]="37";
    data[9]="36";
    data[10]="35";
    data[11]="34";
    data[12]="33";
    data[13]="32";
    data[14]="31";
}

void faker(char *pOut){
    char buffer[50];
    if (IDE == 1)
        strcpy(buffer,data[currentIndex]);
    else
        fgets(buffer,50,stdin);
        
    if ((strlen(buffer) > 0) && (buffer[strlen (buffer) - 1] == '\n'))
        buffer[strlen (buffer) - 1] = '\0';

    // To debug: fprintf(stderr, "Debug messages...\n");
    fprintf(stderr,"data[%d]=\"%s\";\n",currentIndex,buffer);
    currentIndex++;
    strcpy(pOut,buffer);
}

int main()
{

    populate();
    char tmp[50] = "";
    

 // the X position of the light of power
    int light_x;
    // the Y position of the light of power
    int light_y;
    // Thor's starting X position
    int initial_tx;
    // Thor's starting Y position
    int initial_ty;
    faker(tmp);
    sscanf(tmp,"%d%d%d%d", &light_x, &light_y, &initial_tx, &initial_ty);
    
    // game loop
    while (1) {
        // The remaining amount of turns Thor can move. Do not remove this line.
        int remaining_turns;
        faker(tmp);
        sscanf(tmp, "%d", &remaining_turns);
        if (initial_ty < light_y){
            printf("S");
            initial_ty--;
      
        }
        if (initial_ty > light_y){
            printf("N");
            initial_ty++;
            
        }
        if (initial_tx < light_x)
        {
            printf("E");
            initial_tx++;
      
        }
        if (initial_tx > light_x){
            printf("W");
            initial_tx--;
          
        }
        fprintf(stderr, "%d %d\n",initial_tx, initial_ty);
        // Write an action using printf(). DON'T FORGET THE TRAILING \n
        // To debug: fprintf(stderr, "Debug messages...\n");


        // A single line providing the move to be made: N NE E SE S SW W or NW
        printf("\n");
    }

    return 0;
}



