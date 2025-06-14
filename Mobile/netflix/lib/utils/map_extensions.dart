extension CustomUpdation on Map<dynamic, int> {
  int increment(dynamic key) {
    return update(key, (value) => ++value, ifAbsent: () => 1);
  }
  int decrement(dynamic key) {
    return update(key, (value) => --value, ifAbsent: () => 0);
  }
}