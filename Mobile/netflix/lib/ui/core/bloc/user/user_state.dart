class UserState {
  final bool authorized;

  UserState({required this.authorized});

  UserState copyWith({bool? authorized}) {
    return UserState(authorized: authorized ?? this.authorized);
  }
}
