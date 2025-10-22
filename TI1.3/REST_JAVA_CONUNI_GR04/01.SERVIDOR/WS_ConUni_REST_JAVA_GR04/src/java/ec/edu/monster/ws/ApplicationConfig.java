package ec.edu.monster.ws;

import java.util.Set;
import jakarta.ws.rs.ApplicationPath;
import jakarta.ws.rs.core.Application;

@ApplicationPath("webresources")
public class ApplicationConfig extends Application {
    @Override
    public Set<Class<?>> getClasses() {
        Set<Class<?>> resources = new java.util.HashSet<>();
        resources.add(ec.edu.monster.ws.ConUniResource.class);
        resources.add(ec.edu.monster.ws.CORSFilter.class);
        return resources;
    }
}
