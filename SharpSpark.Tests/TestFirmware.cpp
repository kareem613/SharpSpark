int var0 = 0;

void setup() {
	Spark.function("returnOne", returnOne);
	Spark.variable("var0", &var0, INT);
}

int returnOne(String args)
{
	return 1;
}

void loop() {

}