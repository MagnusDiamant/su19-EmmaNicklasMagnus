using DIKUArcade.EventBus;

namespace GalagaGame {
    public static class GalagaBus {
        private static GameEventBus<object> eventBus;
        

        // 2.0 - A method that returns an eventBus if it has already instantiated, otherwise 
        // instantiates it and returns it. 
        public static GameEventBus<object> GetBus() {
            return GalagaBus.eventBus ?? (GalagaBus.eventBus =
                       new GameEventBus<object>());
        }
    }
}