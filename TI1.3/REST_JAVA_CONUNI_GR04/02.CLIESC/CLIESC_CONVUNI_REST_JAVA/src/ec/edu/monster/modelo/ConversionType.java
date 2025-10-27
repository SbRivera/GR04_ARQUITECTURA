package ec.edu.monster.modelo;

public enum ConversionType {
    CM_TO_IN("Centímetros → Pulgadas", "cm-to-in"),
    IN_TO_CM("Pulgadas → Centímetros", "in-to-cm"),
    C_TO_F("Celsius → Fahrenheit", "c-to-f"),
    F_TO_C("Fahrenheit → Celsius", "f-to-c"),
    KG_TO_LB("Kilogramos → Libras", "kg-to-lb"),
    LB_TO_KG("Libras → Kilogramos", "lb-to-kg");

    private final String label;
    private final String path;

    ConversionType(String label, String path) {
        this.label = label;
        this.path = path;
    }

    @Override
    public String toString() {
        return label;
    }

    public String getPath() {
        return path;
    }
}