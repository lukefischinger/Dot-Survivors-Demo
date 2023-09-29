public static class BlueCollisions {
    public static void Hit(Enemy enemy) {
        if (enemy.HasChill()) {
            enemy.chill.Refresh(
                Weapon.blueDamage,
                Weapon.blueSpeedModifier,
                Weapon.blueDuration,
                Weapon.blueDamageDelay,
                Weapon.isBlueCountTrigger,
                Weapon.isBlueMultiHitActive);
            return;
        }

        enemy.chill.gameObject.SetActive(true);
        enemy.chill.SetValues(
            Weapon.blueDamage,
            Weapon.blueSpeedModifier,
            Weapon.blueDuration,
            Weapon.blueDamageDelay,
            Weapon.isBlueCountTrigger,
            Weapon.isBlueMultiHitActive
        );
    }
}
