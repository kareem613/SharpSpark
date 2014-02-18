int var0 = 0;
bool varFalse = false;
char varA = 'A';
double var1dot1 = 1.1;

void setup() {
	Spark.function("returnOne", returnOne);
	Spark.variable("var0", &var0, INT);
	Spark.variable("varFalse", &varFalse, BOOLEAN);
	Spark.variable("varA", &varA, STRING);
	Spark.variable("var1dot1", &varA, DOUBLE);
}

int returnOne(String args)
{
	return 1;
}

void loop() {

}