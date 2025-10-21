package ec.edu.monster.ws;

import jakarta.ws.rs.*;
import jakarta.ws.rs.core.*;

@Path("ConUni")
@Produces(MediaType.APPLICATION_JSON)
public class ConUniResource {

    @Context
    private UriInfo context;

    // ---------- Helper DTO ----------
    public static class Result {
        public String conversion;
        public double input;
        public String inputUnit;
        public double output;
        public String outputUnit;

        public Result() {}
        public Result(String conv, double in, String inU, double out, String outU) {
            this.conversion = conv;
            this.input = in;
            this.inputUnit = inU;
            this.output = out;
            this.outputUnit = outU;
        }
    }

    // ---------- Raíz: describe la API ----------
    @GET
    @Produces(MediaType.TEXT_PLAIN)
    public String info() {
        String base = context.getBaseUriBuilder().path(ConUniResource.class).build().toString();
        return "ConUni REST ✅\n" +
               "Ejemplos:\n" +
               base + "/cm-to-in?value=10\n" +
               base + "/in-to-cm?value=3.94\n" +
               base + "/c-to-f?value=25\n" +
               base + "/f-to-c?value=77\n" +
               base + "/kg-to-lb?value=10\n" +
               base + "/lb-to-kg?value=22.0462\n";
    }

    // ---------- Conversores ----------
    @GET @Path("cm-to-in")
    public Response cmToIn(@QueryParam("value") double v,
                           @HeaderParam("Accept") @DefaultValue(MediaType.APPLICATION_JSON) String accept) {
        double out = v / 2.54d;
        return buildResponse(new Result("Centímetros → Pulgadas", v, "cm", out, "in"), accept);
    }

    @GET @Path("in-to-cm")
    public Response inToCm(@QueryParam("value") double v,
                           @HeaderParam("Accept") @DefaultValue(MediaType.APPLICATION_JSON) String accept) {
        double out = v * 2.54d;
        return buildResponse(new Result("Pulgadas → Centímetros", v, "in", out, "cm"), accept);
    }

    @GET @Path("c-to-f")
    public Response cToF(@QueryParam("value") double v,
                         @HeaderParam("Accept") @DefaultValue(MediaType.APPLICATION_JSON) String accept) {
        double out = v * 9.0d / 5.0d + 32.0d;
        return buildResponse(new Result("Celsius → Fahrenheit", v, "°C", out, "°F"), accept);
    }

    @GET @Path("f-to-c")
    public Response fToC(@QueryParam("value") double v,
                         @HeaderParam("Accept") @DefaultValue(MediaType.APPLICATION_JSON) String accept) {
        double out = (v - 32.0d) * 5.0d / 9.0d;
        return buildResponse(new Result("Fahrenheit → Celsius", v, "°F", out, "°C"), accept);
    }

    @GET @Path("kg-to-lb")
    public Response kgToLb(@QueryParam("value") double v,
                           @HeaderParam("Accept") @DefaultValue(MediaType.APPLICATION_JSON) String accept) {
        double out = v * 2.20462262185d;
        return buildResponse(new Result("Kilogramos → Libras", v, "kg", out, "lb"), accept);
    }

    @GET @Path("lb-to-kg")
    public Response lbToKg(@QueryParam("value") double v,
                           @HeaderParam("Accept") @DefaultValue(MediaType.APPLICATION_JSON) String accept) {
        double out = v / 2.20462262185d;
        return buildResponse(new Result("Libras → Kilogramos", v, "lb", out, "kg"), accept);
    }

    // ---------- Utilidad para JSON o texto ----------
    private Response buildResponse(Result r, String accept) {
        if (accept != null && accept.contains(MediaType.TEXT_PLAIN)) {
            // Solo el número (con 6 decimales) si piden text/plain
            return Response.ok(String.format(java.util.Locale.US, "%.6f", r.output),
                               MediaType.TEXT_PLAIN).build();
        }
        return Response.ok(r, MediaType.APPLICATION_JSON).build();
    }

    // Opcional: endpoint POST con JSON { "value": 12.3 }
    public static class ValueReq { public double value; }
    @POST @Path("cm-to-in")
    @Consumes(MediaType.APPLICATION_JSON)
    public Result cmToInPost(ValueReq req) { return new Result("Centímetros → Pulgadas", req.value, "cm", req.value/2.54d, "in"); }
}
