using UnityEngine;

namespace LogicaInGame.Cartas
{
    public class Cartas{
        
        //misCartas[0]=infanteria; [1]=caballeria; [2]=artilleria
        public int[] misCartas;

        public Cartas(){
            misCartas = new int[3];
            for(int i = 0; i < 3; i++){
                misCartas[i] = 0;
            }
        }

        private Cartas(int n1, int n2, int n3){
            misCartas = new int[3];
            misCartas[0] = n1;
            misCartas[1] = n2;
            misCartas[2] = n3;
        }

        public static int pedirCartaAleatoria(){
            int res = Random.Range(0, 3);
            return res;
        }

        public void addCarta(int tipo){
            misCartas[tipo]++;
        }

        public bool puedoUsarCartas(){
            Cartas posiblesCartas = getCardsToUse();
            
            for(int i= 0; i < 3; i++){
                if (misCartas[i] != 0){
                    return true;
                }
            }

            return false;
        }

        // Devuelve las cartas usadas
        public Cartas usarCartas(){
            Cartas cartas = getCardsToUse();
            
            if(puedoUsarCartas()){
                for( int i=0; i<3; i++){
                    misCartas[i] -= cartas.misCartas[i];
                }
            }
            else{
                Debug.Log("Error en usarCartas: Se ha llamado sin poder usarse, utilice puedoUsarCartas() antes" );
            }

            return cartas;
            
        }

        public Cartas getCardsToUse(){
            if (misCartas[0]>=3){
                return new Cartas(3,0,0);
            }

            if (misCartas[1]>=3){
                return new Cartas(3,0,0);
            }

            if (misCartas[2]>=3){
                return new Cartas(3,0,0);
            }

            if (misCartas[0] > 0 && misCartas[1] > 0 && misCartas[2] > 0){
                return new Cartas(1,1,1);
            }

            return new Cartas();
        }

    }
}