public static class YellowCollisions {
  
    public static void Hit(Enemy enemy) {
        if (enemy.parasite.gameObject.activeInHierarchy) {
            enemy.parasite.Refresh(
                Weapon.yellowSpreadNumber,
                Weapon.yellowDamageMultiplier,
                Weapon.yellowTickLength,
                Weapon.yellowDuration
            );
            return;
        }

        enemy.parasite.gameObject.SetActive(true);
        enemy.parasite.SetValues(
            Weapon.yellowSpreadNumber, 
            Weapon.yellowDamageMultiplier, 
            Weapon.yellowTickLength, 
            Weapon.yellowDuration, 
            Weapon.yellowTickLength
        ); 
    }

    
}
