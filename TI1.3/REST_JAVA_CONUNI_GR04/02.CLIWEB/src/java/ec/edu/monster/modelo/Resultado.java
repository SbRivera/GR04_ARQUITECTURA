/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.modelo;

public class Resultado {
    private String conversion;
    private double input;
    private String inputUnit;
    private double output;
    private String outputUnit;

    public Resultado() {}

    public String getConversion() { return conversion; }
    public void setConversion(String conversion) { this.conversion = conversion; }
    public double getInput() { return input; }
    public void setInput(double input) { this.input = input; }
    public String getInputUnit() { return inputUnit; }
    public void setInputUnit(String inputUnit) { this.inputUnit = inputUnit; }
    public double getOutput() { return output; }
    public void setOutput(double output) { this.output = output; }
    public String getOutputUnit() { return outputUnit; }
    public void setOutputUnit(String outputUnit) { this.outputUnit = outputUnit; }
    
    // Métodos alias para compatibilidad con JSP
    public double getValorOriginal() { return input; }
    public double getValorConvertido() { return output; }
    public String getUnidadOrigen() { return inputUnit; }
    public String getUnidadDestino() { return outputUnit; }
}

